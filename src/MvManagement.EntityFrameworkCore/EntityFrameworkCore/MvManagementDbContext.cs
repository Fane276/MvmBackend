using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Catalogue.Auto;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.MultiTenancy;

namespace MvManagement.EntityFrameworkCore
{
    public class MvManagementDbContext : AbpZeroDbContext<Tenant, Role, User, MvManagementDbContext>, IAutoCatalogueDbContext
    {
        /* Define a DbSet for each entity of the application */
        public MvManagementDbContext(DbContextOptions<MvManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<MakeAuto> MakeAuto { get; }
        public DbSet<MakeAuto> ModelAuto { get; }
    }
}
