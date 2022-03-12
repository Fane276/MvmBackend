using Abp.Application.Services.Dto;

namespace MvManagement.VehicleData.VehicleAccessUtils
{
    public class PermissionAssign
    {
        public string Name { get; set; }
        public long IdVehicle{ get; set; }
        public int? IdRole { get; set; }
        public long? UserId { get; set; }
    }
}