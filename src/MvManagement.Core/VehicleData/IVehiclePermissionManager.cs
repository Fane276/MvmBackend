using System.Threading.Tasks;

namespace MvManagement.VehicleData
{
    public interface IVehiclePermissionManager
    {
        Task<bool> CheckPermission(long vehicleId, string vehiclePermission);
    }
}