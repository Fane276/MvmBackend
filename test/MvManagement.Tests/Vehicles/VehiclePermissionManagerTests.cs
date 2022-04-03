using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.VehicleData.VehicleAccessUtils;
using Shouldly;
using Xunit;

namespace MvManagement.Tests.Vehicles
{
    public class VehiclePermissionManagerTests : MvManagementTestBase
    {
        private readonly IVehiclePermissionManager _vehiclePermissionManager;

        public VehiclePermissionManagerTests()
        {
            _vehiclePermissionManager = Resolve<IVehiclePermissionManager>();
        }
        [Fact]
        public async Task CheckPermission_Test()
        {
            var output = await _vehiclePermissionManager.CheckCurrentUserPermissionAsync(1, VehiclePermissionNames.VehicleInfo.View);
            
            output.ShouldBeFalse();

            await _vehiclePermissionManager.AsignPermissionAndGetIdAsync(new PermissionAssign()
                {IdVehicle = 1, UserId = AbpSession.UserId, Name = VehiclePermissionNames.VehicleInfo.View});
            
            output = await _vehiclePermissionManager.CheckCurrentUserPermissionAsync(1, VehiclePermissionNames.VehicleInfo.View);

            output.ShouldBeTrue();
        }

        [Fact]
        public async Task CreateRole_Test()
        {
            await Assert.ThrowsAsync<AbpException>(() => _vehiclePermissionManager.CreateRoleAndGetIdAsync(
                new VehicleRole()
                {
                    Name = "Driver",
                    TenantId = 0,
                },
                0,
                new List<string> {VehiclePermissionNames.VehicleInfo.View, VehiclePermissionNames.VehicleInfo.Edit, "not defined permission"}
            ));


            var idRole = await _vehiclePermissionManager.CreateRoleAndGetIdAsync(
                new VehicleRole()
                {
                    Name = "Driver",
                    TenantId = 0,
                },
                0,
                new List<string> {VehiclePermissionNames.VehicleInfo.View, VehiclePermissionNames.VehicleInfo.Edit}
            );

            idRole.ShouldNotBe(0);
        }
        [Fact]
        public async Task AsignPermissionToRole_Test()
        {
            await Assert.ThrowsAsync<AbpException>(() => _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = "permission",
                    IdRole = 1,
                    IdVehicle = 1
                }
            ));
            var permissionRepository = Resolve<IRepository<VehiclePermission, long>>();

            await permissionRepository.InsertAsync(new VehiclePermission()
            {
                Name = VehiclePermissionNames.VehicleInfo.View
            });

            var idPermission = await _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    IdRole = 1,
                    IdVehicle = 1
                }
            );

            idPermission.ShouldNotBe(0);

            await Assert.ThrowsAsync<AbpException>(() => _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    IdRole = 1,
                    IdVehicle = 1
                }
            ));
        }
        [Fact]
        public async Task AsignPermissionToUser_Test()
        {
            await Assert.ThrowsAsync<AbpException>(() => _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = "permission",
                    UserId = 1,
                    IdVehicle = 1
                }
            ));
            var permissionRepository = Resolve<IRepository<VehiclePermission, long>>();

            await permissionRepository.InsertAsync(new VehiclePermission()
            {
                Name = VehiclePermissionNames.VehicleInfo.View
            });

            var idPermission = await _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    UserId = 1,
                    IdVehicle = 1
                }
            );

            idPermission.ShouldNotBe(0);

            await Assert.ThrowsAsync<AbpException>(() => _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    UserId = 1,
                    IdVehicle = 1
                }
            ));

            var hasPermission = await _vehiclePermissionManager.CheckPermissionAsync(1, 1, VehiclePermissionNames.VehicleInfo.View);

            hasPermission.ShouldBeTrue();

        }

        [Fact]
        public async Task DeletePermissionFromRole_Test()
        {

            var permissionRepository = Resolve<IRepository<VehiclePermission, long>>();

            await permissionRepository.InsertAsync(new VehiclePermission()
            {
                Name = VehiclePermissionNames.VehicleInfo.View
            });

            var idPermission = await _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    IdRole = 1,
                    IdVehicle = 1
                }
            );
            idPermission.ShouldNotBe(0);

            await _vehiclePermissionManager.DeletePermissionFromRoleAsync(1, 1, VehiclePermissionNames.VehicleInfo.Edit);
            var permission = await permissionRepository.FirstOrDefaultAsync(p => p.IdVehicle == 1 && p.IdRole == 1);
            permission.ShouldNotBeNull();

            await _vehiclePermissionManager.DeletePermissionFromRoleAsync(1, 1, VehiclePermissionNames.VehicleInfo.View);
            permission = await permissionRepository.FirstOrDefaultAsync(p => p.IdVehicle == 1 && p.IdRole == 1);
            permission.ShouldBeNull();
        }
        [Fact]
        public async Task DeletePermissionFromUser_Test()
        {

            var permissionRepository = Resolve<IRepository<VehiclePermission, long>>();

            await permissionRepository.InsertAsync(new VehiclePermission()
            {
                Name = VehiclePermissionNames.VehicleInfo.View
            });

            var idPermission = await _vehiclePermissionManager.AsignPermissionAndGetIdAsync(
                new PermissionAssign()
                {
                    Name = VehiclePermissionNames.VehicleInfo.View,
                    UserId = 1,
                    IdVehicle = 1
                }
            );
            idPermission.ShouldNotBe(0);

            await _vehiclePermissionManager.DeletePermissionFromUserAsync(1, 1, VehiclePermissionNames.VehicleInfo.View);

            var permission = await permissionRepository.FirstOrDefaultAsync(p => p.IdVehicle == 1 && p.UserId == 1);
            permission.ShouldBeNull();
        }
    }
}