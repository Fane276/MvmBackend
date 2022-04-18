using Abp.Application.Services.Dto;
using Catalogue.Auto;

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
        public int? IdMakeAuto { get; set; }
        public string MakeAuto { get; set; }
        public int? IdModelAuto { get; set; }
        public string ModelAuto { get; set; }
        public AutoTypeCategoryMake VehicleType { get; set; }
    }
}