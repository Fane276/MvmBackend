using System;

namespace MvManagement.Documents.PeriodicalDocuments.Dto
{
    public class PeriodicalDocumentInput
    {
        public int IdPeriodicalDocumentType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long IdVehicle { get; set; }
    }
}