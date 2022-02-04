using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MvMangement.Authorization.Roles;
using MvMangement.Authorization.Users;
using MvMangement.MultiTenancy;

namespace MvMangement.EntityFrameworkCore
{
    public class MvMangementDbContext : AbpZeroDbContext<Tenant, Role, User, MvMangementDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MvMangementDbContext(DbContextOptions<MvMangementDbContext> options)
            : base(options)
        {
        }
    }
}
