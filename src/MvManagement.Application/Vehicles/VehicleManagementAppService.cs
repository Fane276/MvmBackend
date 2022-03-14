using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.ObjectMapping;
using Microsoft.EntityFrameworkCore;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.VehicleData;
using MvManagement.VehicleData.VehicleAccess;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Vehicles
{
    public class VehicleManagementAppService : VehicleAppServiceBase, IVehicleManagementAppService
    {
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<VehicleRoleUser, long> _vehicleRoleUserRepository;
        private readonly IRepository<VehiclePermission, long> _vehiclePermissionRepository;


        public VehicleManagementAppService(
            IVehiclePermissionManager vehiclePermissionManager,
            IObjectMapper mapper, 
            IRepository<Vehicle, long> vehicleRepository, 
            IRepository<VehicleRoleUser, long> vehicleRoleUserRepository, 
            IRepository<VehiclePermission, long> vehiclePermissionRepository) : base(vehiclePermissionManager, mapper)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleRoleUserRepository = vehicleRoleUserRepository;
            _vehiclePermissionRepository = vehiclePermissionRepository;
        }

        public async Task<PagedResultDto<VehicleDto>> GetCurrentUserPersonalVehiclesAsync(VehiclesPagedResultRequestDto input)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            var listOfVehicles = await _vehicleRepository.GetAll()
                .Where(v => v.UserId == currentUserId)
                .Select(v => Mapper.Map<VehicleDto>(v))
                .ToListAsync();

            listOfVehicles = listOfVehicles.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                v => v.Title.Contains(input.Filter, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return new PagedResultEnumerableDto<VehicleDto>(listOfVehicles, input).Get();
        }


        public async Task<PagedResultDto<VehicleDto>> GetTenantVehiclesAsync(VehiclesPagedResultRequestDto input)
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            if (AbpSession.TenantId == null)
            {
                throw new AbpException("Tenant not specified");
            }

            var idsVehiclesUserHasAccess = await _vehicleRoleUserRepository.GetAll()
                .Join(
                    _vehiclePermissionRepository.GetAll(),
                    ur => ur.IdRole,
                    p => p.IdRole,
                    (ur, p) => new {userRole = ur, vehiclePermission = p}
                )
                .Where(o => (o.userRole.UserId == currentUserId || o.vehiclePermission.UserId == currentUserId) &&
                            o.vehiclePermission.Name.Equals(VehiclePermissionNames.VehicleInfo.View))
                .Select(o => o.vehiclePermission.IdVehicle)
                .ToListAsync();

            var listOfVehicles = await _vehicleRepository.GetAll()
                .Where(v => v.TenantId == AbpSession.TenantId && !idsVehiclesUserHasAccess.Contains(v.Id))
                .Select(v => Mapper.Map<VehicleDto>(v))
                .ToListAsync();

            listOfVehicles = listOfVehicles.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                v => v.Title.Contains(input.Filter, StringComparison.InvariantCultureIgnoreCase)).ToList();

            return new PagedResultEnumerableDto<VehicleDto>(listOfVehicles, input).Get();
        }

        public async Task<long> CreateVehicleAsync(VehicleDto input)
        {
            var entity = Mapper.Map<Vehicle>(input);

            return await _vehicleRepository.InsertAndGetIdAsync(entity);
        }
        public async Task UpdateVehicleAsync(VehicleDto input)
        {
            var entity = Mapper.Map<Vehicle>(input);

            await _vehicleRepository.UpdateAsync(entity);
        }
    }
}