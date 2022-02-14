using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.Auto
{
    [Table("tblCatAutoMake", Schema = "cat")]
    public class MakeAuto : Entity
    {
        [Key]
        [Required]
        [Column("IdMake")]
        public int Id { get; set; }

        public int IdExtern { get; set; }

        public ProviderAutoCatalogue Provider { get; set; }

        [Required]
        [Column("Make")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}