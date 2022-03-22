using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Documents.Insurance
{
    public interface IInsuranceAppService
    {
        Task SaveInsuranceAsync(InsuranceDocumentDto document);
        Task<PagedResultDto<InsuranceDocumentDto>> GetIsurancesForVehicleAsync(VehiclesPagedResultRequestDto input);
    }
}