using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehicleAccess", Schema = "veh")]
    public class VehicleAccess : FullAuditedEntity<long>, IMayHaveTenant, IUserIdentifier
    {
        [Key]
        [Required]
        [Column("IdVehicle")]
        public override long Id { get; set; }
        public int? TenantId { get; set; }
        [Required]
        [Column("IdUser")]
        public long UserId { get; }

        [Required]
        [Column("IdUser")]
        [ForeignKey(nameof(VehicleRole))]
        public int IdRole { get; set; }

        public VehicleRole VehicleRole { get; set; }
    }
}