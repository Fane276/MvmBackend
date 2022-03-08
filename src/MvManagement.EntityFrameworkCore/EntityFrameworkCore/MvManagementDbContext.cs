using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Catalogue.Auto;
using Catalogue.Documents;
using Catalogue.InsuranceCompany;
using MvManagement.Authorization.Roles;
using MvManagement.Authorization.Users;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.PeriodicalDocuments;
using MvManagement.Documents.StorageDocuments;
using MvManagement.MultiTenancy;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;

namespace MvManagement.EntityFrameworkCore
{
    public class MvManagementDbContext : AbpZeroDbContext<Tenant, Role, User, MvManagementDbContext>,
        IAutoCatalogueDbContext,
        IDocumentsCatalogueDbContext,
        IInsuranceCompanyCatalogueDbContext
    {
        public MvManagementDbContext(DbContextOptions<MvManagementDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VehicleRolePermission>()
                .HasKey(nameof(VehicleRolePermission.IdRole), nameof(VehicleRolePermission.IdPermission));
        }

        #region Catalogues
        public DbSet<MakeAuto> MakeAuto { get; }
        public DbSet<ModelAuto> ModelAuto { get; }
        public DbSet<MakeCategoryAuto> MakeCategoryAuto { get; }
        public DbSet<InsuranceCompany> InsuranceCompany { get; }
        public DbSet<PeriodicalDocumentType> PeriodicalDocumentTypes { get; }
        public DbSet<StorageDocumentType> StorageDocumentType { get; }
        #endregion

        public DbSet<Vehicle> Vehicle { get; }

        #region VehicleAccess
        public DbSet<VehicleAccess> VehicleAccess { get; }
        public DbSet<VehiclePermission> VehiclePermission { get; }
        public DbSet<VehicleRole> VehicleRole { get; }
        public DbSet<VehicleRolePermission> VehicleRolePermissions { get; }
        #endregion

        #region Documents

        public DbSet<InsuranceDocument> InsuranceDocument { get; }
        public DbSet<PeriodicalDocument> PeriodicalDocument { get; }
        public DbSet<StorageDocument> StorageDocument { get; }

        #endregion
    }
}
