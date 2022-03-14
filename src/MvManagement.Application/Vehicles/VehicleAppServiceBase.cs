using Abp.ObjectMapping;
using MvManagement.VehicleData;

namespace MvManagement.Vehicles
{
    public class VehicleAppServiceBase : MvManagementAppServiceBase
    {
        protected readonly IVehiclePermissionManager VehiclePermissionManager;
        protected readonly IObjectMapper Mapper;

        public VehicleAppServiceBase(IVehiclePermissionManager vehiclePermissionManager, IObjectMapper mapper)
        {
            VehiclePermissionManager = vehiclePermissionManager;
            Mapper = mapper;
        }

    }
}