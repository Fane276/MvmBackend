using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.MultiTenancy;

namespace MvManagement.EntityFrameworkCore
{
    public class MvManagementDbContext : AbpZeroDbContext<Tenant, Role, User, MvManagementDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public MvManagementDbContext(DbContextOptions<MvManagementDbContext> options)
            : base(options)
        {
        }
    }
}
