using System;
using Abp.Application.Services.Dto;

namespace MvManagement.Documents.UserDocuments.Dto
{
    public class UserDocumentDto : EntityDto<long> 
    {
        public UserDocumentType DocumentType { get; set; }
        public string OtherDocumentType { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int TenantId { get; set; }
        public long UserId { get; set; }
    }
}