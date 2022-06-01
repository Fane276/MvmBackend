using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Catalogue.Auto;
using JetBrains.Annotations;
using MvManagement.Authorization.Users;

namespace MvManagement.Vehicles.Dto
{
    public class VehicleCreateDto : EntityDto<long>
    {
        public string Title { get; set; }
        public int ProductionYear { get; set; }
        public string RegistrationNumber { get; set; }
        public string ChassisNo { get; set; }
        public int? TenantId { get; set; }
        public int? IdMakeAuto { get; set; }
        public string OtherAutoMake { get; set; }
        public int? IdModelAuto { get; set; }
        public string OtherAutoModel { get; set; }
        public long? UserId { get; set; }
    }
}