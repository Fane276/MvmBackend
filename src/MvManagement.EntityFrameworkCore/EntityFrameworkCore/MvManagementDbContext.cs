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
using MvManagement.FuelManagement;
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
        #region Catalogues
        public DbSet<MakeAuto> MakeAuto { get; set; }
        public DbSet<ModelAuto> ModelAuto { get; set; }
        public DbSet<MakeCategoryAuto> MakeCategoryAuto { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompany { get; set; }
        public DbSet<PeriodicalDocumentType> PeriodicalDocumentTypes { get; set; }
        public DbSet<StorageDocumentType> StorageDocumentType { get; set; }
        #endregion

        public DbSet<Vehicle> Vehicle { get; set; }

        #region VehicleAccess
        public DbSet<VehiclePermission> VehiclePermission { get; set; }
        public DbSet<VehicleRole> VehicleRole { get; set; }
        public DbSet<VehicleRoleUser> VehicleRoleUser { get; set; }
        #endregion

        #region Documents

        public DbSet<InsuranceDocument> InsuranceDocument { get; set; }
        public DbSet<PeriodicalDocument> PeriodicalDocument { get; set; }
        public DbSet<StorageDocument> StorageDocument { get; set; }

        #endregion
        public DbSet<FuelRefill> FuelRefill { get; set; }
        public MvManagementDbContext(DbContextOptions<MvManagementDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<VehicleRoleUser>()
                .HasIndex(p => new { p.UserId, p.IdVehicle }).IsUnique();
        }

    }
}
