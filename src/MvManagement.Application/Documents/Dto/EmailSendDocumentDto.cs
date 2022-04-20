using System;

namespace MvManagement.Documents.Dto
{
    public class EmailSendDocumentDto
    {
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public DocumentType DocumentType { get; set; }
        public string VehicleTitle { get; set; }
        public string RegistrationNumber { get; set; }
        public int TenantId { get; set; }
        public long UserId { get; set; }
    }
}