using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehicleRolePermission", Schema = "veh")]
    public class VehicleRolePermission : IFullAudited
    {
        [Key]
        [Required]
        [ForeignKey(nameof(VehiclePermission))]
        [Column("IdVehiclePermission")]
        public int IdPermission { get; set; }
        public VehiclePermission VehiclePermission { get; set; }
        [Key]
        [Required]
        [ForeignKey(nameof(VehicleRole))]
        [Column("IdVehicleRole")]
        public int IdRole { get; set; }
        public VehicleRole VehicleRole { get; set; }

        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public long? DeleterUserId { get; set; }
    }
}