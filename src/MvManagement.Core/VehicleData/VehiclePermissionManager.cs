using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MvManagement.Authorization.Users;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.VehicleData.VehicleAccessUtils;

namespace MvManagement.VehicleData
{
    public class VehiclePermissionManager : ApplicationService, IVehiclePermissionManager
    {
        private readonly IRepository<VehiclePermission, long> _vehiclePermissionRepository;
        private readonly IRepository<VehicleRole> _vehicleRoleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRoleUserRepository;
        private readonly UserManager _userManager;

        public VehiclePermissionManager(IRepository<VehiclePermission, long> vehiclePermissionRepository, IRepository<VehicleRole> vehicleRoleRepository, UserManager userManager, IRepository<VehicleRoleUser, long> vehicleRoleUserRepository)
        {
            _vehiclePermissionRepository = vehiclePermissionRepository;
            _vehicleRoleRepository = vehicleRoleRepository;
            _userManager = userManager;
            _vehicleRoleUserRepository = vehicleRoleUserRepository;
        }

        public async Task<bool> CheckCurrentUserPermissionAsync(long vehicleId, string vehiclePermission)
        {
            if (!_userManager.AbpSession.UserId.HasValue)
            {
                throw new AuthenticationException("User not authenticated!");
            }

            var userId = _userManager.AbpSession.UserId;

            return await CheckPermissionAsync((long)userId, vehicleId, vehiclePermission);
        }

        public async Task<bool> CheckPermissionAsync(long userId, long vehicleId, string vehiclePermission)
        {
            var idRoleOfUserPerCar = await _vehicleRoleUserRepository.GetAll()
                .Where(r => r.IdVehicle == vehicleId && r.UserId == userId)
                .Select(r=>r.IdRole)
                .FirstOrDefaultAsync();

            if (idRoleOfUserPerCar == 0)
            {
                var userPermission = await _vehiclePermissionRepository.GetAll()
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.IdVehicle == vehicleId && p.Name.Equals(vehiclePermission));

                return userPermission != null;
            }

            var rolePermission = await _vehiclePermissionRepository.GetAll()
                .Join(
                    _vehicleRoleUserRepository.GetAll(),
                    p=>p.IdRole,
                    vp=>vp.IdRole,
                    (p, vp)=>new {permission = p, userClaim = vp}
                    )
                .Where(p => p.permission.IdRole == idRoleOfUserPerCar &&
                            p.userClaim.IdVehicle == vehicleId &&
                            p.userClaim.UserId == userId &&
                            p.permission.Name.Equals(vehiclePermission))
                .FirstOrDefaultAsync();
            return rolePermission != null;
        }

        public async Task<long> AsignPermissionAndGetIdAsync(PermissionAssign input)
        {
            var permission = await _vehiclePermissionRepository.GetAll()
                .FirstOrDefaultAsync(p => p.Name.Equals(input.Name));
            if (permission == null)
            {
                throw new AbpException("Permission does not exist");
            }

            if (input.UserId != null)
            {

                var hasPermission = await CheckPermissionAsync((long)input.UserId, input.IdVehicle, input.Name);
                if (hasPermission)
                {
                    throw new AbpException("User already have this permission");
                }
                
                return await _vehiclePermissionRepository.InsertOrUpdateAndGetIdAsync(new VehiclePermission()
                {
                    UserId = input.UserId,
                    Name = input.Name,
                    IdVehicle = input.IdVehicle,
                    Description = permission.Description
                });
            }

            if (input.IdRole == null)
            {
                throw new AbpException("IdRole or UserId should be defined");
            }


            var existsPermissionOnRole = await _vehiclePermissionRepository.GetAll()
                .FirstOrDefaultAsync(p => 
                    p.Name.Equals(input.Name) && p.IdVehicle == input.IdVehicle && p.IdRole == input.IdRole) != null;

            if (existsPermissionOnRole)
            {
                throw new AbpException("User role already have this permission");
            }
            return await _vehiclePermissionRepository.InsertOrUpdateAndGetIdAsync(new VehiclePermission()
            {
                IdRole = input.IdRole,
                Name = input.Name,
                IdVehicle = input.IdVehicle,
                Description = permission.Description
            });
        }

        
        public async Task<int> CreateRoleAndGetIdAsync(VehicleRole vehicleRole, long idVehicle, List<string> vehicleRolePermissions)
        {
            var allPermissionsOnTenant = await _vehiclePermissionRepository.GetAll().Select(p=>p.Name).ToListAsync();

            var permissionsNotAvailable = vehicleRolePermissions
                .Any(p => !allPermissionsOnTenant.Contains(p));

            if (permissionsNotAvailable)
            {
                throw new AbpException("One of the requested permission is not defined");
            }


            var idRole = await _vehicleRoleRepository.InsertAndGetIdAsync(vehicleRole);

            foreach (var permission in vehicleRolePermissions)
            {
                await _vehiclePermissionRepository.InsertAsync(new VehiclePermission()
                {
                    IdRole = idRole,
                    Name = permission,
                    IdVehicle = idVehicle,
                });
            }

            return idRole;
        }

        public async Task DeletePermissionFromRoleAsync(int idRole, long idVehicle, string permissionName)
        {
            await _vehiclePermissionRepository.DeleteAsync(p => p.IdRole == idRole && p.IdVehicle == idVehicle && p.Name == permissionName);
        }
        public async Task DeletePermissionFromUserAsync(int idUser, long idVehicle, string permissionName)
        {
            await _vehiclePermissionRepository.DeleteAsync(p => p.UserId == idUser && p.IdVehicle == idVehicle && p.Name == permissionName);
        }

        public async Task<IEnumerable<VehiclePermission>> GetCurrentUserPermissions(long idVehicle)
        {
            if (!_userManager.AbpSession.UserId.HasValue)
            {
                throw new AuthenticationException("User not authenticated!");
            }

            var userId = _userManager.AbpSession.UserId;

            var result = new List<VehiclePermission>();

            var permissionsPerUser = await _vehiclePermissionRepository.GetAll()
                .Where(p => p.IdVehicle == idVehicle && p.UserId == userId)
                .ToListAsync();

            result.AddRange(permissionsPerUser);

            var userRole = await _vehicleRoleUserRepository.GetAll()
                .Where(p => p.UserId == userId && p.IdVehicle == idVehicle)
                .FirstOrDefaultAsync();

            if (userRole != null)
            {
                var rolePermissions = await _vehiclePermissionRepository.GetAll()
                    .Where(p => p.IdVehicle == idVehicle && p.IdRole == userRole.IdRole)
                    .ToListAsync();

                result.AddRange(rolePermissions);
            }

            return result;
        }
        public async Task<IEnumerable<VehiclePermission>> GetRolePermissions(int idRole, long idVehicle)
        {
            var rolePermissions = await _vehiclePermissionRepository.GetAll()
                .Where(p => p.IdVehicle == idVehicle && p.IdRole == idRole)
                .ToListAsync();

            return rolePermissions;
        }

        public async Task AsignUserRoleAsync(long idUser, string roleName, long idVehicle)
        {
            var role = await _vehicleRoleRepository.FirstOrDefaultAsync(r => r.Name.Equals(roleName));

            if (role == null)
            {
                throw new NullReferenceException("Specified role does not exist");
            }

            await _vehicleRoleUserRepository.InsertAsync(new VehicleRoleUser()
            {
                IdRole = role.Id,
                IdVehicle = idVehicle,
                UserId = idUser,
            });
        }

        public async Task AsignCurrentUserRoleAsync(string roleName, long idVehicle)
        {
            var currentUserId = AbpSession.UserId;
            if (currentUserId == null)
            {
                throw new AuthenticationException("User not authenticated!");
            }

            await AsignUserRoleAsync((long)currentUserId, roleName, idVehicle);
        }
    }
}