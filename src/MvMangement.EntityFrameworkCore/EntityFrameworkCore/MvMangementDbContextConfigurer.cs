using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MvMangement.EntityFrameworkCore
{
    public static class MvMangementDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MvMangementDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MvMangementDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
