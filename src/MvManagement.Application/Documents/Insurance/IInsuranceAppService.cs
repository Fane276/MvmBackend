using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Documents.Dto;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Documents.Insurance
{
    public interface IInsuranceAppService
    {
        Task SaveInsuranceAsync(InsuranceDocumentDto document);
        Task<InsuranceResultDto> GetInsurancesForVehicleAsync(long idVehicle);
        Task<InsuranceIdsResultDto> GetInsuranceIdsForVehicleAsync(long idVehicle);
        Task DeleteInsurance(long idInsurance);
        Task EditInsuranceAsync(InsuranceDocumentDto document);
        Task<List<ExpiredDocumentDto>> GetExpiredInsuranceForAllUserVehiclesAsync();
    }
}