using Microsoft.EntityFrameworkCore;

namespace Catalogue.Auto
{
    public interface IAutoCatalogueDbContext 
    {
        DbSet<MakeAuto> MakeAuto { get; }
        DbSet<MakeAuto> ModelAuto { get; }
    }
}