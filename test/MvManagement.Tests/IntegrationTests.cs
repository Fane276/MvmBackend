using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;
using Shouldly;
using Xunit;

namespace MvManagement.Tests
{
    public class IntegrationTests : MvManagementTestBase
    {
        private readonly IVehicleManagementAppService _vehicleManagement;
        private readonly IInsuranceAppService _insuranceAppService;

        public IntegrationTests()
        {
            _insuranceAppService = Resolve<IInsuranceAppService>();
            _vehicleManagement = Resolve<IVehicleManagementAppService>();
        }

        [Fact]
        public async Task AddInsuranceTest()
        {
            var vehicle = new VehicleCreateDto()
            {
                ChassisNo = "01234567891234567",
                ProductionYear = 2021,
                RegistrationNumber = "SB27ABC",
                Title = "new car"
            };
            var idVehicle = await _vehicleManagement.CreateVehicleAsync(vehicle);
            idVehicle.ShouldNotBe(0);
            await _insuranceAppService.SaveInsuranceAsync(new InsuranceDocumentDto()
            {
                IdInsuranceCompany = 1,
                IdVehicle = idVehicle,
                InsurancePolicyNumber = "123",
                InsuranceType = InsuranceType.Rca,
                ValidFrom = DateTime.Today,
                ValidTo = DateTime.Now,
            });

            var insuranceStatus = await _insuranceAppService.GetInsurancesForVehicleAsync(idVehicle);
            insuranceStatus.Rca.ShouldNotBeNull();
        }
    }
}