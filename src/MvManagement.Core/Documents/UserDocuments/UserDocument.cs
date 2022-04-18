using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using JetBrains.Annotations;
using MvManagement.Authorization.Users;

namespace MvManagement.Documents.UserDocuments
{
    [Table("tblUserDocuments", Schema = "doc")]
    public class UserDocument : FullAuditedEntity<long>, IMustHaveTenant
    {
        [Key]
        [Column("IdUserDocument")]
        public override long Id { get; set; }

        public UserDocumentType DocumentType { get; set; }
        [CanBeNull]
        public string OtherDocumentType { get; set; }
        
        public DateTime ValidFrom { get; set; }
        [CanBeNull]
        public DateTime? ValidTo { get; set; }


        public int TenantId { get; set; }

        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}