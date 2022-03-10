using Abp.Application.Services.Dto;

namespace MvManagement.VehicleData.VehicleAccessUtils
{
    public class UserPermissionAssign : EntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TenantId { get; set; }
        public int UserId { get; set; }
    }
}