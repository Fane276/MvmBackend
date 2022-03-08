using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catalogue.Documents;

namespace MvManagement.Documents.StorageDocuments
{
    [Table("tblStorageDocument", Schema = "doc")]
    public class StorageDocument : DocumentAudited
    {
        [Key]
        [Required]
        [Column("IdStorageDocument")]
        public long Id { get; set; }

        [Required]
        [ForeignKey(nameof(StorageDocumentType))]
        public int IdStorageDocumentType { get; set; }
        public StorageDocumentType StorageDocumentType { get; set; }
    }
}