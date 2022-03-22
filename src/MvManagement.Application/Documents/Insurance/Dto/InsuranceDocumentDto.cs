using System;
using Abp.Application.Services.Dto;

namespace MvManagement.Documents.Insurance.Dto
{
    public class InsuranceDocumentDto : EntityDto<long>
    {
        public InsuranceType InsuranceType { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public int IdInsuranceCompany { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public long IdVehicle { get; set; }
    }
}