using System.IO;
using System.Threading.Tasks;
using Abp;
using Abp.UI;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;
using Shouldly;
using Xunit;

namespace MvManagement.Tests.Vehicles
{
    public class VehicleManagementTests : MvManagementTestBase
    {
        private readonly IVehicleManagementAppService _vehicleManagement;

        public VehicleManagementTests()
        {
            _vehicleManagement = Resolve<IVehicleManagementAppService>();
        }

        [Fact]
        public async Task VehicleCreate_Test()
        {
            //var vehicle = new VehicleCreateDto()
            //{
            //    ChassisNo = "012345678912345678", // only 17 characters are allowed
            //    ProductionYear = 2000,
            //    RegistrationNumber = "SB27ABC",
            //    Title = "new car"
            //};

            //await Assert.ThrowsAsync<InvalidDataException>(() => _vehicleManagement.CreateVehicleAsync(vehicle));

            //vehicle = new VehicleCreateDto()
            //{
            //    ChassisNo = "0123456789123456", // only 17 characters are allowed
            //    ProductionYear = 2000,
            //    RegistrationNumber = "SB27ABC",
            //    Title = "new car"
            //};

            //await Assert.ThrowsAsync<InvalidDataException>(() => _vehicleManagement.CreateVehicleAsync(vehicle));

            var vehicle = new VehicleCreateDto()
            {
                ChassisNo = "01234567891234567",
                ProductionYear = 200,
                RegistrationNumber = "SB27ABC",
                Title = "new car"
            };
            await Assert.ThrowsAsync<UserFriendlyException>(() => _vehicleManagement.CreateVehicleAsync(vehicle));

            vehicle = new VehicleCreateDto()
            {
                ChassisNo = "01234567891234567",
                ProductionYear = 2200,
                RegistrationNumber = "SB27ABC",
                Title = "new car"
            };
            await Assert.ThrowsAsync<UserFriendlyException>(() => _vehicleManagement.CreateVehicleAsync(vehicle));

            vehicle = new VehicleCreateDto()
            {
                ChassisNo = "01234567891234567",
                ProductionYear = 1886,
                RegistrationNumber = "SB27ABC",
                Title = "new car"
            };
            var idVehicle = await _vehicleManagement.CreateVehicleAsync(vehicle);
            idVehicle.ShouldNotBe(0);

            vehicle = new VehicleCreateDto()
            {
                ChassisNo = "01234567891234567",
                ProductionYear = 2021,
                RegistrationNumber = "SB27ABC",
                Title = "new car"
            };
            idVehicle = await _vehicleManagement.CreateVehicleAsync(vehicle);
            idVehicle.ShouldNotBe(0);
            
        }
    }
}