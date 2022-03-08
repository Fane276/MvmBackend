using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.Documents
{
    [Table("tblCatStorageDocumentType", Schema = "cat")]
    public class StorageDocumentType : Entity
    {
        [Key]
        [Required]
        [Column("IdStorageDocumentType")]
        public int Id { get; set; }

        [Required]
        [Column("StorageDocumentType")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}