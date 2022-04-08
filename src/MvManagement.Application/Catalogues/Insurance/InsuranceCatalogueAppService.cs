using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
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

        public async Task<ListResultDto<SelectListItem>> GetInsuranceCompanies(string q)
        {
            var insuranceCompanies = await _insuranceCompanyRepository.GetAll()
                .Select(c=>new SelectListItem(){Text = c.Name, Value = c.Id.ToString()})
                .ToListAsync();
            var companies = insuranceCompanies
                .WhereIf(!q.IsNullOrWhiteSpace(), c => c.Text.Contains(q, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            return new ListResultDto<SelectListItem>(companies);
        }
    }
}