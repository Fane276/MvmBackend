using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.UI;
using Catalogue.Auto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Vehicles
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class VehicleManagementAppService : VehicleAppServiceBase, IVehicleManagementAppService
    {
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRoleUserRepository;
        private readonly IRepository<VehiclePermission, long> _vehiclePermissionRepository;
        private readonly IRepository<MakeAuto> _makeAutoRepository;
        private readonly IRepository<ModelAuto> _modelAutoRepository;


        public VehicleManagementAppService(
            IVehiclePermissionManager vehiclePermissionManager,
            IRepository<Vehicle, long> vehicleRepository, 
            IRepository<VehicleRoleUser, long> vehicleRoleUserRepository, 
            IRepository<VehiclePermission, long> vehiclePermissionRepository, 
            IRepository<MakeAuto> makeAutoRepository,
            IRepository<ModelAuto> modelAutoRepository) : base(vehiclePermissionManager)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleRoleUserRepository = vehicleRoleUserRepository;
            _vehiclePermissionRepository = vehiclePermissionRepository;
            _makeAutoRepository = makeAutoRepository;
            _modelAutoRepository = modelAutoRepository;
        }

        public async Task<PagedResultDto<VehicleDto>> GetCurrentUserPersonalVehiclesAsync(PagedSortedAndFilteredInputDto input)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            var listOfVehicles = await (from vehicle in _vehicleRepository.GetAll()
                where vehicle.UserId == currentUserId
                join makeAuto in _makeAutoRepository.GetAll() on vehicle.IdMakeAuto equals makeAuto.Id into make
                from makeOrDefault in make.DefaultIfEmpty()
                join modelAuto in _modelAutoRepository.GetAll() on vehicle.IdModelAuto equals modelAuto.Id into model
                from modelOrDefault in model.DefaultIfEmpty()
                select new VehicleDto()
                {
                    Id = vehicle.Id,
                    UserId = vehicle.UserId,
                    TenantId = vehicle.TenantId,
                    ChassisNo = vehicle.ChassisNo,
                    ProductionYear = vehicle.ProductionYear,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    Title = vehicle.Title,
                    IdMakeAuto = vehicle.IdMakeAuto,
                    IdModelAuto = vehicle.IdMakeAuto,
                    MakeAuto = makeOrDefault.Name,
                    ModelAuto = modelOrDefault.Name
                })
                .ToListAsync();

            listOfVehicles = listOfVehicles.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                v => v.Title.Contains(input.Filter, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return new PagedResultEnumerableDto<VehicleDto>(listOfVehicles, input).Get();
        }

        public async Task<PagedResultDto<VehicleDto>> GetTenantVehiclesAsync(PagedSortedAndFilteredInputDto input)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            if (AbpSession.TenantId == null)
            {
                throw new AbpException("Tenant not specified");
            }

            var idsVehiclesUserHasAccess = await _vehicleRoleUserRepository.GetAll()
                .Join(
                    _vehiclePermissionRepository.GetAll(),
                    ur => ur.IdRole,
                    p => p.IdRole,
                    (ur, p) => new {userRole = ur, vehiclePermission = p}
                )
                .Where(o => (o.userRole.UserId == currentUserId || o.vehiclePermission.UserId == currentUserId) &&
                            o.vehiclePermission.Name.Equals(VehiclePermissionNames.VehicleInfo.View))
                .Select(o => o.vehiclePermission.IdVehicle)
                .ToListAsync();

            var listOfVehicles = await (from vehicle in _vehicleRepository.GetAll()
                    where vehicle.UserId == currentUserId && !idsVehiclesUserHasAccess.Contains(vehicle.Id)
                    join makeAuto in _makeAutoRepository.GetAll() on vehicle.IdMakeAuto equals makeAuto.Id into make
                    from makeOrDefault in make.DefaultIfEmpty()
                    join modelAuto in _modelAutoRepository.GetAll() on vehicle.IdModelAuto equals modelAuto.Id into model
                    from modelOrDefault in model.DefaultIfEmpty()
                    select new VehicleDto()
                    {
                        Id = vehicle.Id,
                        UserId = vehicle.UserId,
                        TenantId = vehicle.TenantId,
                        ChassisNo = vehicle.ChassisNo,
                        ProductionYear = vehicle.ProductionYear,
                        RegistrationNumber = vehicle.RegistrationNumber,
                        Title = vehicle.Title,
                        IdMakeAuto = vehicle.IdMakeAuto,
                        IdModelAuto = vehicle.IdMakeAuto,
                        MakeAuto = makeOrDefault.Name,
                        ModelAuto = modelOrDefault.Name
                    })
                .ToListAsync();

            listOfVehicles = listOfVehicles.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                v => v.Title.Contains(input.Filter, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return new PagedResultEnumerableDto<VehicleDto>(listOfVehicles, input).Get();
        }

        public async Task<long> CreateVehicleAsync(VehicleCreateDto input)
        {
            if (input.ProductionYear < 1886 || input.ProductionYear > DateTime.Today.Year)
            {
                throw new UserFriendlyException("Vehicle year is incorrect");
            }

            var entity = ObjectMapper.Map<Vehicle>(input);

            if (input.TenantId == null)
            {
                entity.UserId = AbpSession.UserId;
                entity.TenantId = 1; // default tenant
            }

            var vehicleId =  await _vehicleRepository.InsertAndGetIdAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            await VehiclePermissionManager.AsignCurrentUserRoleAsync("Owner", vehicleId);

            return vehicleId;
        }
        public async Task UpdateVehicleAsync(VehicleDto input)
        {
            var entity = ObjectMapper.Map<Vehicle>(input);

            await _vehicleRepository.UpdateAsync(entity);
        }

        public async Task DeleteVehicleAsync(long idVehicle)
        {
            await _vehicleRepository.DeleteAsync(v => v.Id == idVehicle);
        }
    }
}