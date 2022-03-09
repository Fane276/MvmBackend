using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using MvManagement.Authorization.Users;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehiclePermission", Schema = "veh")]
    public class VehiclePermission : Entity, ICreationAudited, IMayHaveTenant
    {
        [Key]
        [Required]
        [Column("IdVehiclePermission")]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        [Column("Name")]
        public int Name { get; set; }
        [CanBeNull]
        [Column("PermissionDescription")]
        public string Description { get; set; }
        public int? TenantId { get; set; }

        [CanBeNull]
        [ForeignKey(nameof(Vehicle))]
        [Column("IdVehicle")]
        public int? IdVehicle { get; set; }
        public Vehicle Vehicle { get; set; }
        [CanBeNull]
        [ForeignKey(nameof(VehicleRole))]
        [Column("IdVehicleRole")]
        public int? IdRole { get; set; }
        public VehicleRole VehicleRole { get; set; }
        [CanBeNull]
        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public int? UserId { get; set; }
        public User User { get; set; }

    }
}