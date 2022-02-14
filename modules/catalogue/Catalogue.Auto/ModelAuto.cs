using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.Auto
{
    [Table("tblCatAutoModel", Schema = "cat")]
    public class ModelAuto : Entity
    {
        [Key]
        [Required]
        [Column("IdModel")]
        public int Id { get; set; }

        [Required]
        [Column("Model")]
        [MaxLength(250)]
        public string Name { get; set; }

        [ForeignKey(nameof(Make))]
        public virtual int IdMake { get; set; }
        public virtual MakeAuto Make { get; set; }
    }
}