using Abp.ObjectMapping;
using MvManagement.VehicleData;

namespace MvManagement.Vehicles
{
    public class VehicleAppServiceBase : MvManagementAppServiceBase
    {
        protected readonly IVehiclePermissionManager VehiclePermissionManager;
        public VehicleAppServiceBase(IVehiclePermissionManager vehiclePermissionManager)
        {
            VehiclePermissionManager = vehiclePermissionManager;
        }

    }
}