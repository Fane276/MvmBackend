using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;

namespace MvManagement.VehicleData
{
    [Table("tblVehicle", Schema = "veh")]
    public class Vehicle : FullAuditedEntity<long>
    {
        [Key]
        [Required]
        [Column("IdVehicle")]
        public long Id { get; set; }

        [CanBeNull]
        [MaxLength(250)]
        [Column("VehicleTitle")]
        public string Title { get; set; }

        [Required]
        public int ProductionYear{ get; set; }
        
        [MaxLength(6)]
        [Column("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [CanBeNull]
        [MaxLength(17)]
        [Column("ChassisNumber")]
        public string ChassisNo { get; set; }
    }
}