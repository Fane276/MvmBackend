using Microsoft.EntityFrameworkCore;

namespace Catalogue.Documents
{
    public interface IDocumentsCatalogueDbContext
    {
        DbSet<PeriodicalDocumentType> PeriodicalDocumentTypes { get; set; }
        DbSet<StorageDocumentType> StorageDocumentType { get; set; }
    }
}