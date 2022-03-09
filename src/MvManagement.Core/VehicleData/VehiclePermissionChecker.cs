using System.Data;
using System.Linq;
using System.Security.Authentication;
using System.Security.Policy;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MvManagement.Authorization.Users;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.VehicleData.VehicleAccessUtils;

namespace MvManagement.VehicleData
{
    public class VehiclePermissionChecker : ApplicationService, IVehiclePermissionChecker
    {
        private readonly IRepository<VehiclePermission> _vehiclePermissionRepository;
        private readonly IRepository<VehicleRole> _vehicleRoleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRoleUserRepository;
        private readonly UserManager _userManager;

        public VehiclePermissionChecker(IRepository<VehiclePermission> vehiclePermissionRepository, IRepository<VehicleRole> vehicleRoleRepository, UserManager userManager, IRepository<VehicleRoleUser, long> vehicleRoleUserRepository)
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
                IdRole = input.IdRole,
                Name = input.Name,
                Description = input.Description,
                TenantId = input.TenantId
            });
        }
    }
}