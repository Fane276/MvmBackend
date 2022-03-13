using Microsoft.EntityFrameworkCore;

namespace Catalogue.Auto
{
    public interface IAutoCatalogueDbContext 
    {
        DbSet<MakeAuto> MakeAuto { get; set; }
        DbSet<ModelAuto> ModelAuto { get; set; }
        DbSet<MakeCategoryAuto> MakeCategoryAuto { get; set; }
    }
}