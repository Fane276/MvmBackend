using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using MvManagement.Authorization;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.Editions;
using MvManagement.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;

namespace MvManagement.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;
        private readonly IRepository<VehicleRole> _vehicleRoleRepository;
        private readonly IRepository<VehiclePermission, long> _vehiclePermissionRepository;

        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator, 
            IRepository<VehicleRole> vehicleRoleRepository, 
            IRepository<VehiclePermission, long> vehiclePermissionRepository)
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;
            _vehicleRoleRepository = vehicleRoleRepository;
            _vehiclePermissionRepository = vehiclePermissionRepository;
        }

        public override async Task<TenantDto> CreateAsync(CreateTenantDto input)
        {
            CheckCreatePermission();

            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            // Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                await _userManager.InitializeOptionsAsync(tenant.Id);
                CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();

                await GenerateDefaultVehicleRoles(tenant.Id);
            }

            return MapToEntityDto(tenant);
        }

        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public async Task GenerateDefaultVehicleRoles(int tenantId)
        {
            using (CurrentUnitOfWork.SetTenantId(tenantId))
            {
                var listOfPredefinedRoles = new List<VehicleRole>();
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = tenantId, Name = "Owner", Description = "Implicit assigned when adding a vehicle and have all the permissions" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = tenantId, Name = "Administrator", Description = "Have all the permissions to the vehicle including adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = tenantId, Name = "Operator", Description = "Have all the permissions to the vehicle without adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = tenantId, Name = "AdvanceDriver", Description = "Have access to see and edit all the documents" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = tenantId, Name = "Driver", Description = "Have access to see all the documents but can not edit them" });
                listOfPredefinedRoles.ForEach(async role => await _vehicleRoleRepository.InsertAsync(role));
                await CurrentUnitOfWork.SaveChangesAsync();

                var listOfPermissionAssignment = new List<VehiclePermission>();

                // Start - Owner role
                var ownerRoleId = await _vehicleRoleRepository.GetAll().Where(p => p.TenantId == tenantId && p.Name.Equals("Owner")).Select(r => r.Id).FirstOrDefaultAsync();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.UserRoles.Add });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.UserRoles.Remove });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.UserRoles.Update });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = ownerRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Owner role
                // Start - Administrator role
                var administratorRoleId = await _vehicleRoleRepository.GetAll().Where(p => p.TenantId == tenantId && p.Name.Equals("Administrator")).Select(r => r.Id).FirstOrDefaultAsync();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.UserRoles.Add });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.UserRoles.Remove });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.UserRoles.Update });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = administratorRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Administrator role
                // Start - Operator role
                var operatorRoleId = await _vehicleRoleRepository.GetAll().Where(p => p.TenantId == tenantId && p.Name.Equals("Operator")).Select(r => r.Id).FirstOrDefaultAsync();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = operatorRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Operator role
                // Start - AdvanceDriver role
                var advenceDriverRoleId = await _vehicleRoleRepository.GetAll().Where(p => p.TenantId == tenantId && p.Name.Equals("AdvanceDriver")).Select(r => r.Id).FirstOrDefaultAsync();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - AdvanceDriver role
                // Start - Driver role
                var driverRoleId = await _vehicleRoleRepository.GetAll().Where(p => p.TenantId == tenantId && p.Name.Equals("Driver")).Select(r => r.Id).FirstOrDefaultAsync();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                // End - Driver role
                listOfPermissionAssignment.ForEach(async permission => await _vehiclePermissionRepository.InsertAsync(permission));
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            
        }
    }
}

