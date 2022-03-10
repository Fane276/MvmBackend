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
    public class VehiclePermission : Entity<long>, ICreationAudited
    {
        [Key]
        [Required]
        [Column("IdVehiclePermission")]
        public override long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        [Column("Name")]
        public string Name { get; set; }
        [CanBeNull]
        [Column("PermissionDescription")]
        public string Description { get; set; }

        [CanBeNull]
        [ForeignKey(nameof(Vehicle))]
        [Column("IdVehicle")]
        public long? IdVehicle { get; set; }
        public Vehicle Vehicle { get; set; }
        [CanBeNull]
        [ForeignKey(nameof(VehicleRole))]
        [Column("IdVehicleRole")]
        public int? IdRole { get; set; }
        public VehicleRole VehicleRole { get; set; }
        [CanBeNull]
        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public long? UserId { get; set; }
        public User User { get; set; }

    }
}