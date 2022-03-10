using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Security.Policy;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Castle.Core.Internal;
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

        public async Task<bool> CheckPermission(long vehicleId, string vehiclePermission)
        {
            if (!_userManager.AbpSession.UserId.HasValue)
            {
                throw new AuthenticationException("User not authenticated!");
            }

            var userId = _userManager.AbpSession.UserId;

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
                .FirstOrDefaultAsync(p => p.IdRole == idRoleOfUserPerCar && p.IdVehicle == vehicleId && p.Name.Equals(vehiclePermission));
            return rolePermission != null;
        }

        public async Task AsignOrUpdatePermissionToRole(RolePermissionAssign input)
        {
            await _vehiclePermissionRepository.InsertOrUpdateAsync(new VehiclePermission()
            {
                Id = input.Id,
                IdRole = input.IdRole,
                Name = input.Name,
                Description = input.Description,
                TenantId = input.TenantId
            });
        }
        public async Task AsignOrUpdatePermissionToUser(UserPermissionAssign input)
        {
            await _vehiclePermissionRepository.InsertOrUpdateAsync(new VehiclePermission()
            {
                Id = input.Id,
                UserId = input.UserId,
                Name = input.Name,
                Description = input.Description,
                TenantId = input.TenantId
            });
        }

        public async Task CreateRoleAsync(VehicleRole vehicleRole, long idVehicle, List<string> vehicleRolePermissions)
        {
            var idRole = await _vehicleRoleRepository.InsertAndGetIdAsync(vehicleRole);

            foreach (var permission in vehicleRolePermissions)
            {
                await _vehiclePermissionRepository.InsertAsync(new VehiclePermission()
                {
                    IdRole = idRole,
                    Name = permission,
                    IdVehicle = idVehicle,
                    TenantId = vehicleRole.TenantId
                });
            }
        }

        public async Task DeletePermissionFromRoleAsync(int idRole, string permissionName)
        {
            await _vehiclePermissionRepository.DeleteAsync(p => p.IdRole == idRole && p.Name == permissionName);
        }
        public async Task DeletePermissionFromUserAsync(int idUser, string permissionName)
        {
            await _vehiclePermissionRepository.DeleteAsync(p => p.UserId == idUser && p.Name == permissionName);
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
    }
}