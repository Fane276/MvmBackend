using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Catalogue.Documents;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MvManagement.Catalogues.PeriodicalDocuments
{
    public class PeriodicalDocumentTypeAppService : MvManagementAppServiceBase, IPeriodicalDocumentTypeAppService
    {
        private readonly IRepository<PeriodicalDocumentType> _periodicalDocumentTypeRepository;

        public PeriodicalDocumentTypeAppService(IRepository<PeriodicalDocumentType> periodicalDocumentTypeRepository)
        {
            _periodicalDocumentTypeRepository = periodicalDocumentTypeRepository;
        }
        public async Task<ListResultDto<SelectListItem>> GetPeriodicalDocumentsTypes(string q)
        {
            var insuranceCompanies = await _periodicalDocumentTypeRepository.GetAll()
                .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
                .ToListAsync();
            var companies = insuranceCompanies
                .WhereIf(!q.IsNullOrWhiteSpace(), c => c.Text.Contains(q, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return new ListResultDto<SelectListItem>(companies);
        }
    }
}