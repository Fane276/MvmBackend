using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehicleRole", Schema = "veh")]
    public class VehicleRole : FullAuditedEntity, IMayHaveTenant
    {
        [Key]
        [Required]
        [Column("IdVehicleRole")]
        public override int Id { get; set; }
        [Required]
        [Column("Name")]
        public string Name { get; set; }
        [CanBeNull]
        [Column("Description")]
        public string Description { get; set; }

        public int? TenantId { get; set; }
    }
}