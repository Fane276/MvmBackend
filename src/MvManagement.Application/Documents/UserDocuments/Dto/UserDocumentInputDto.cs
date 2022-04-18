using System;

namespace MvManagement.Documents.UserDocuments.Dto
{
    public class UserDocumentInputDto
    {
        public UserDocumentType DocumentType { get; set; }
        public string OtherDocumentType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public long? UserId { get; set; }
    }
}