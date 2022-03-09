using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using MvManagement.Authorization.Users;
using MvManagement.VehicleData.VehicleAccess;

namespace MvManagement.VehicleData
{
    public class VehiclePermissionChecker : ApplicationService
    {
        private readonly IRepository<VehiclePermission> _vehiclePermissionRepository;
        private readonly IRepository<VehicleRole> _vehicleRoleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRolePermissionRepository;

        public VehiclePermissionChecker(IRepository<VehiclePermission> vehiclePermissionRepository, IRepository<VehicleRole> vehicleRoleRepository, IRepository<VehicleRoleUser, long> vehicleRolePermissionRepository)
        {
            _vehiclePermissionRepository = vehiclePermissionRepository;
            _vehicleRoleRepository = vehicleRoleRepository;
            _vehicleRolePermissionRepository = vehicleRolePermissionRepository;
        }

        public async Task<bool> CheckPermission(long vehicleId, string vehiclePermission)
        {


            return false;
        }
    }
}