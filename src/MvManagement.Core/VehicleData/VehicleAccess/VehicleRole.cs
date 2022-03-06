using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using JetBrains.Annotations;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehicleRole", Schema = "veh")]
    public class VehicleRole : Entity
    {
        [Key]
        [Required]
        [Column("IdVehicleRole")]
        public int Id { get; set; }
        [Required]
        [Column("VehicleRole")]
        public int Title { get; set; }
        [CanBeNull]
        [Column("RoleDescription")]
        public string Description { get; set; }
    }
}