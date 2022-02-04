using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MvManagement.EntityFrameworkCore
{
    public static class MvManagementDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<MvManagementDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<MvManagementDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
