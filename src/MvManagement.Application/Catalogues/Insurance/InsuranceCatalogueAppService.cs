using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Catalogue.InsuranceCompany;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MvManagement.Catalogues.Insurance
{
    public class InsuranceCatalogueAppService : MvManagementAppServiceBase, IInsuranceCatalogueAppService
    {
        private readonly IRepository<InsuranceCompany> _insuranceCompanyRepository;

        public InsuranceCatalogueAppService(IRepository<InsuranceCompany> insuranceCompanyRepository)
        {
            _insuranceCompanyRepository = insuranceCompanyRepository;
        }

        public async Task<List<SelectListItem>> GetInsuranceCompanies(string filter)
        {
            var insuranceCompanies = await _insuranceCompanyRepository.GetAll()
                .Select(c=>new SelectListItem(){Text = c.Name, Value = c.Id.ToString()})
                .ToListAsync();
            return insuranceCompanies
                .WhereIf(!filter.IsNullOrWhiteSpace(), c => c.Text.Contains(filter))
                .ToList();
        }
    }
}