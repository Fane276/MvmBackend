using Microsoft.EntityFrameworkCore;

namespace Catalogue.InsuranceCompany
{
    public interface IInsuranceCompanyCatalogueDbContext
    {
        DbSet<InsuranceCompany> InsuranceCompany { get; }
    }
}