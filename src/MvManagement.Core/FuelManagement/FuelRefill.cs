using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using MvManagement.VehicleData;

namespace MvManagement.FuelManagement
{
    [Table("tblFuelRefill", Schema = "veh")]
    public class FuelRefill : CreationAuditedEntity<long>
    {
        [Key]
        [Required]
        [Column("IdFuelRefill")]
        public override long Id { get; set; }
        public FuelType FuelType { get; set; }
        public double FuelAmount { get; set; }
        public FuelUnit FuelUnit { get; set; }
        public double Price { get; set; }
        [Required]
        [ForeignKey(nameof(Vehicle))]
        public long IdVehicle { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}