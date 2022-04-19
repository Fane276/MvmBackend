using System;
using Abp.Application.Services.Dto;

namespace MvManagement.Documents.PeriodicalDocuments.Dto
{
    public class PeriodicalDocumentDto : EntityDto<long>
    {
        public int IdPeriodicalDocumentType { get; set; }
        public string PeriodicalDocumentType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long IdVehicle { get; set; }
    }
}