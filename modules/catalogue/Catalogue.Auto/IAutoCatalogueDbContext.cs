using Microsoft.EntityFrameworkCore;

namespace Catalogue.Auto
{
    public interface IAutoCatalogueDbContext 
    {
        DbSet<MakeAuto> MakeAuto { get; }
        DbSet<ModelAuto> ModelAuto { get; }
        DbSet<MakeCategoryAuto> MakeCategoryAuto { get; }
    }
}