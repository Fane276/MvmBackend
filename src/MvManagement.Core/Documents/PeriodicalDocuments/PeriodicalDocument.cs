using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catalogue.Documents;
using MvManagement.VehicleData;

namespace MvManagement.Documents.PeriodicalDocuments
{
    [Table("tblPeriodicalDocument", Schema = "doc")]
    public class PeriodicalDocument : DatedDocumentAudited
    {
        [Key]
        [Required]
        [Column("IdPeriodicalDocument")]
        public long Id { get; set; }

        [Required]
        [ForeignKey(nameof(PeriodicalDocumentType))]
        public int IdPeriodicalDocumentType { get; set; }
        public PeriodicalDocumentType PeriodicalDocumentType { get; set; }
    }
}