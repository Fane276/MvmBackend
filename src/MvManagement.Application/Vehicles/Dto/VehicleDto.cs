using Abp.Application.Services.Dto;

namespace MvManagement.Vehicles.Dto
{
    public class VehicleDto : EntityDto<long>
    {
        public string Title { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationNumber { get; set; }
        public string ChassisNo { get; set; }
        public int? TenantId { get; set; }
        public long? UserId { get; set; }
    }
}