using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.Documents
{
    [Table("tblCatPeriodicalDocumentType", Schema = "cat")]
    public class PeriodicalDocumentType : Entity<int>
    {
        [Key]
        [Required]
        [Column("IdPeriodicalDocumentType")]
        public int Id { get; set; }

        [Required]
        [Column("PeriodicalDocumentType")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}