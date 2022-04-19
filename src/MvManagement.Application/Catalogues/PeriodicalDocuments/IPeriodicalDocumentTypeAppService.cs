using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvManagement.Catalogues.PeriodicalDocuments
{
    public interface IPeriodicalDocumentTypeAppService
    {
        Task<ListResultDto<SelectListItem>> GetPeriodicalDocumentsTypes(string q);
    }
}