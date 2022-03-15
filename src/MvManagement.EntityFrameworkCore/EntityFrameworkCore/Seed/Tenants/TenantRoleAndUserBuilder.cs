using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Castle.Core.Internal;
using MvManagement.Authorization;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;

namespace MvManagement.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly MvManagementDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(MvManagementDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
            GenerateDefaultVehicleRoles();
        }

        private void CreateRolesAndUsers()
        {
            // Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role

            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId && p.RoleId == adminRole.Id)
                .Select(p => p.Name)
                .ToList();

            var permissions = PermissionFinder
                .GetAllPermissions(new MvManagementAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }

            // Admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }

        }
        public void GenerateDefaultVehicleRoles()
        {
            var generateRoles = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId).ToList().IsNullOrEmpty();
            if (generateRoles)
            {
                var listOfPredefinedRoles = new List<VehicleRole>();
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = _tenantId, Name = "Owner", Description = "Implicit assigned when adding a vehicle and have all the permissions" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = _tenantId, Name = "Administrator", Description = "Have all the permissions to the vehicle including adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = _tenantId, Name = "Operator", Description = "Have all the permissions to the vehicle without adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = _tenantId, Name = "AdvanceDriver", Description = "Have access to see and edit all the documents" });
                listOfPredefinedRoles.Add(new VehicleRole { TenantId = _tenantId, Name = "Driver", Description = "Have access to see all the documents but can not edit them" });
                _context.VehicleRole.AddRange(listOfPredefinedRoles);
                _context.SaveChanges();

                var listOfPermissionAssignment = new List<VehiclePermission>();

                // Start - Owner role
                var ownerRoleId = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId && p.Name.Equals("Owner")).Select(r=>r.Id).FirstOrDefault();
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
                var administratorRoleId = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId && p.Name.Equals("Administrator")).Select(r => r.Id).FirstOrDefault();
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
                var operatorRoleId = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId && p.Name.Equals("Operator")).Select(r => r.Id).FirstOrDefault();
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
                var advenceDriverRoleId = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId && p.Name.Equals("AdvanceDriver")).Select(r => r.Id).FirstOrDefault();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = advenceDriverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - AdvanceDriver role
                // Start - Driver role
                var driverRoleId = _context.VehicleRole.IgnoreQueryFilters().Where(p => p.TenantId == _tenantId && p.Name.Equals("Driver")).Select(r => r.Id).FirstOrDefault();
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { IdRole = driverRoleId, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                // End - Driver role
                _context.VehiclePermission.AddRange(listOfPermissionAssignment);
                _context.SaveChanges();
            }
        }
    }
}
