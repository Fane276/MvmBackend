using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Catalogue.Documents;
using Catalogue.InsuranceCompany;
using Microsoft.EntityFrameworkCore;
using MvManagement.Documents.PeriodicalDocuments;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;

namespace MvManagement.EntityFrameworkCore.Seed.Vehicle
{
    public class DefaultVehicleDataBuilder
    {

        private readonly MvManagementDbContext _context;

        public DefaultVehicleDataBuilder(MvManagementDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateVehicleDocumentsCategories();
            CreateIsuranceCompanies();
            CreateDefaultVehiclePermissions();
            GenerateDefaultVehicleRoles();
        }

        private void CreateVehicleDocumentsCategories()
        {
            var generatePeriodcal = _context.PeriodicalDocumentTypes.Where(p=>p.Id!=0).ToList().IsNullOrEmpty();
            if (generatePeriodcal)
            {
                var listOfTypes = new List<PeriodicalDocumentType>();
                listOfTypes.Add(new PeriodicalDocumentType {Id = 1, Name = "PeriodicalDocument.Itp"});
                listOfTypes.Add(new PeriodicalDocumentType {Id = 2, Name = "PeriodicalDocument.FireExtinguisher"});
                listOfTypes.Add(new PeriodicalDocumentType {Id = 3, Name = "PeriodicalDocument.Rovienieta"});
                listOfTypes.Add(new PeriodicalDocumentType {Id = 4, Name = "PeriodicalDocument.MedicalKit"});
                

                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatPeriodicalDocumentType ON");
                _context.PeriodicalDocumentTypes.AddRange(listOfTypes);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatPeriodicalDocumentType OFF");
            }

            var generateStorage = _context.PeriodicalDocumentTypes.Where(p => p.Id != 0).ToList().IsNullOrEmpty();
            if (generateStorage)
            {
                var listOfStorageTypes = new List<StorageDocumentType>();
                listOfStorageTypes.Add(new StorageDocumentType { Id = 1, Name = "StorageDocumentType.Civ"});
                listOfStorageTypes.Add(new StorageDocumentType { Id = 2, Name = "StorageDocumentType.Talon"}); // talon
                listOfStorageTypes.Add(new StorageDocumentType { Id = 3, Name = "StorageDocumentType.DrivingLicense"});
                listOfStorageTypes.Add(new StorageDocumentType { Id = 4, Name = "StorageDocumentType.DriverId" });
                listOfStorageTypes.Add(new StorageDocumentType { Id = 5, Name = "StorageDocumentType.Homologation" });

                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatStorageDocumentType ON");
                _context.StorageDocumentType.AddRange(listOfStorageTypes);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatStorageDocumentType OFF");
            }
        }

        private void CreateIsuranceCompanies()
        {
            var generateCompanies = _context.InsuranceCompany.Where(p => p.Id != 0).ToList().IsNullOrEmpty();
            if (generateCompanies)
            {
                var listOfCompanies = new List<InsuranceCompany>();
                listOfCompanies.Add(new InsuranceCompany {Id = 1, Name = "Allianz-Tiriac" });
                listOfCompanies.Add(new InsuranceCompany {Id = 2, Name = "Asirom" });
                listOfCompanies.Add(new InsuranceCompany {Id = 3, Name = "Euroins" });
                listOfCompanies.Add(new InsuranceCompany {Id = 4, Name = "Generali" });
                listOfCompanies.Add(new InsuranceCompany {Id = 5, Name = "Groupama" });
                listOfCompanies.Add(new InsuranceCompany {Id = 6, Name = "Omniasig" });
                listOfCompanies.Add(new InsuranceCompany {Id = 7, Name = "Uniqa" });
                listOfCompanies.Add(new InsuranceCompany {Id = 8, Name = "Grawe" });
                listOfCompanies.Add(new InsuranceCompany {Id = 9, Name = "Axeria" });

                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatInsuranceCompany ON");
                _context.InsuranceCompany.AddRange(listOfCompanies);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT cat.tblCatInsuranceCompany OFF");
            }
        }

        private void CreateDefaultVehiclePermissions()
        {
            var defaultPermissionExist = _context.VehiclePermission.IgnoreQueryFilters().Any(p => p.IdVehicle == null && p.IdRole == null && p.UserId == null);
            if (!defaultPermissionExist)
            {
                var listOfPermissions = new List<VehiclePermission>();
                listOfPermissions.Add(new VehiclePermission { Id = 1, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissions.Add(new VehiclePermission { Id = 2, Name = VehiclePermissionNames.UserRoles.Add });
                listOfPermissions.Add(new VehiclePermission { Id = 3, Name = VehiclePermissionNames.UserRoles.Remove });
                listOfPermissions.Add(new VehiclePermission { Id = 4, Name = VehiclePermissionNames.UserRoles.Update });
                listOfPermissions.Add(new VehiclePermission { Id = 5, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissions.Add(new VehiclePermission { Id = 6, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissions.Add(new VehiclePermission { Id = 7, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissions.Add(new VehiclePermission { Id = 8, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissions.Add(new VehiclePermission { Id = 9, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissions.Add(new VehiclePermission { Id = 10, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissions.Add(new VehiclePermission { Id = 11, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissions.Add(new VehiclePermission { Id = 12, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });

                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehiclePermission ON");
                _context.VehiclePermission.AddRange(listOfPermissions);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehiclePermission OFF");
            }
        }

        public void GenerateDefaultVehicleRoles()
        {
            var generateRoles = _context.VehicleRole.Where(p => p.Id != 0).ToList().IsNullOrEmpty();
            if (generateRoles)
            {
                var listOfPredefinedRoles = new List<VehicleRole>();
                listOfPredefinedRoles.Add(new VehicleRole { Id = 1, Name = "Owner", Description = "Implicit assigned when adding a vehicle and have all the permissions" });
                listOfPredefinedRoles.Add(new VehicleRole { Id = 2, Name = "Administrator", Description = "Have all the permissions to the vehicle including adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { Id = 3, Name = "Operator", Description = "Have all the permissions to the vehicle without adding other roles" });
                listOfPredefinedRoles.Add(new VehicleRole { Id = 4, Name = "AdvanceDriver", Description = "Have access to see and edit all the documents" });
                listOfPredefinedRoles.Add(new VehicleRole { Id = 5, Name = "Driver", Description = "Have access to see all the documents but can not edit them" });
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehicleRole ON");
                _context.VehicleRole.AddRange(listOfPredefinedRoles);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehicleRole OFF");

                var listOfPermissionAssignment = new List<VehiclePermission>();
                // Start - Owner role
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 13, IdRole = 1, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 14, IdRole = 1, Name = VehiclePermissionNames.UserRoles.Add });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 15, IdRole = 1, Name = VehiclePermissionNames.UserRoles.Remove });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 16, IdRole = 1, Name = VehiclePermissionNames.UserRoles.Update });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 17, IdRole = 1, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 18, IdRole = 1, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 19, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 20, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 21, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 22, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 23, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 24, IdRole = 1, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Owner role
                // Start - Administrator role
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 25, IdRole = 2, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 26, IdRole = 2, Name = VehiclePermissionNames.UserRoles.Add });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 27, IdRole = 2, Name = VehiclePermissionNames.UserRoles.Remove });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 28, IdRole = 2, Name = VehiclePermissionNames.UserRoles.Update });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 29, IdRole = 2, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 30, IdRole = 2, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 31, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 32, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 33, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 34, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 35, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 36, IdRole = 2, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Administrator role
                // Start - Operator role
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 37, IdRole = 3, Name = VehiclePermissionNames.UserRoles.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 38, IdRole = 3, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 39, IdRole = 3, Name = VehiclePermissionNames.VehicleInfo.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 40, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 41, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 42, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 43, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 44, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 45, IdRole = 3, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - Operator role
                // Start - AdvanceDriver role
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 46, IdRole = 4, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 47, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 48, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.Insurance.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 49, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 50, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.Edit });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 51, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 52, IdRole = 4, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.Edit });
                // End - AdvanceDriver role
                // Start - Driver role
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 53, IdRole = 5, Name = VehiclePermissionNames.VehicleInfo.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 54, IdRole = 5, Name = VehiclePermissionNames.VehicleDocuments.Insurance.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 55, IdRole = 5, Name = VehiclePermissionNames.VehicleDocuments.PeriodicalDocuments.View });
                listOfPermissionAssignment.Add(new VehiclePermission { Id = 56, IdRole = 5, Name = VehiclePermissionNames.VehicleDocuments.StorageDocuments.View });
                // End - Driver role
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehiclePermission ON");
                _context.VehiclePermission.AddRange(listOfPermissionAssignment);
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT veh.tblVehiclePermission OFF");
            }

        }
    }
}