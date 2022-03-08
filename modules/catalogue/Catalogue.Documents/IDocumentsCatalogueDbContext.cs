using Microsoft.EntityFrameworkCore;

namespace Catalogue.Documents
{
    public interface IDocumentsCatalogueDbContext
    {
        DbSet<PeriodicalDocumentType> PeriodicalDocumentTypes { get; }
        DbSet<StorageDocumentType> StorageDocumentType { get; }
    }
}