using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.Auto
{
    [Table("tblCatAutoMakeCategory", Schema = "cat")]
    public class AutoMakeCategory : Entity
    {
        [Key]
        [Required]
        [Column("IdMakeCategory")]
        public override int Id { get; set; }

        [ForeignKey(nameof(Make))]
        public virtual int IdMake { get; set; }
        public virtual MakeAuto Make { get; set; }

        public AutoTypeCategoryMake Category { get; set; }
    }
}