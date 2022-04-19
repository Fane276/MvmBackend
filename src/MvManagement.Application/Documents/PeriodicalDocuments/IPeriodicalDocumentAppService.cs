using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Documents.Dto;
using MvManagement.Documents.PeriodicalDocuments.Dto;

namespace MvManagement.Documents.PeriodicalDocuments
{
    public interface IPeriodicalDocumentAppService
    {
        Task<ListResultDto<PeriodicalDocumentDto>> GetPeriodicalDocumentsAsync(long idVehicle);
        Task<long> AddPeriodicalDocumentAsync(PeriodicalDocumentInput input);
        Task DeletePeriodicalDocumentAsync(DeletePeriodicalDocumentInput input);
        Task UpdatePeriodicalDocumentAsync(PeriodicalDocumentDto input);
        Task<PeriodicalDocumentDto> GetPeriodicalDocumentAsync(long idDocument);
        Task<List<ExpiredDocumentDto>> GetExpiredPeriodicalDocumentsAllUserVehiclesAsync();
    }
}