using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using JetBrains.Annotations;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehiclePermission", Schema = "veh")]
    public class VehiclePermission : Entity
    {
        [Key]
        [Required]
        [Column("IdVehiclePermission")]
        public int Id { get; set; }
        [Required]
        [Column("VehiclePermission")]
        public int PermissionKey { get; set; }
        [CanBeNull]
        [Column("PermissionDescription")]
        public string Description { get; set; }
    }
}