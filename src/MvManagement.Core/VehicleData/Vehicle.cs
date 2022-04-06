using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Catalogue.Auto;
using JetBrains.Annotations;
using MvManagement.Authorization.Users;

namespace MvManagement.VehicleData
{
    [Table("tblVehicle", Schema = "veh")]
    public class Vehicle : FullAuditedEntity<long>, IMayHaveTenant
    {
        [Key]
        [Required]
        [Column("IdVehicle")]
        public override long Id { get; set; }

        [CanBeNull]
        [MaxLength(250)]
        [Column("VehicleTitle")]
        public string Title { get; set; }

        [Required]
        public int ProductionYear{ get; set; }
        
        [MaxLength(7)]
        [Column("RegistrationNumber")]
        public string RegistrationNumber { get; set; }

        [CanBeNull]
        [MaxLength(17)]
        [Column("ChassisNumber")]
        public string ChassisNo { get; set; }
        public int? TenantId { get; set; }

        [CanBeNull]
        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public long? UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey(nameof(MakeAuto))]
        [Column("IdMakeAuto")]
        public int? IdMakeAuto { get; set; }
        public MakeAuto MakeAuto{ get; set; }
        public string OtherAutoMake { get; set; }

        [ForeignKey(nameof(ModelAuto))]
        [Column("IdModelAuto")]
        public int? IdModelAuto { get; set; }
        public ModelAuto ModelAuto { get; set; }
        public string OtherAutoModel { get; set; }
    }
}