using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using MvManagement.VehicleData;

namespace MvManagement.Documents
{
    public class DocumentAudited<T> : FullAuditedEntity<T>
    {
        public Guid ImageRef { get; set; }

        [Required]
        [ForeignKey(nameof(Vehicle))]
        public long IdVehicle { get; set; }
        public Vehicle Vehicle { get; set; }
    }

    public class DocumentAudited : DocumentAudited<int>
    {
    }
}