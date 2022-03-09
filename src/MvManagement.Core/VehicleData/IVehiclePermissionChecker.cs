using System.Threading.Tasks;

namespace MvManagement.VehicleData
{
    public interface IVehiclePermissionChecker
    {
        Task<bool> CheckPermission(long vehicleId, string vehiclePermission);
    }
}