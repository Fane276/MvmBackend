using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MvManagement.Extensions.Dto.PageResult;
using MvManagement.FuelManagement.Dto;
using MvManagement.VehicleData;
using MvManagement.Vehicles;
using MvManagement.Vehicles.Dto;

namespace MvManagement.FuelManagement
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class FuelManagementAppService : VehicleAppServiceBase, IFuelManagementAppService
    {
        private readonly IRepository<FuelRefill, long> _fuelRefillRepository;

        public FuelManagementAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<FuelRefill, long> fuelRefillRepository
            ) : base(vehiclePermissionManager)
        {
            _fuelRefillRepository = fuelRefillRepository;
        }

        public async Task<PagedResultDto<FuelRefillDto>> GetVehicleRefillsAsync(VehiclesPagedResultRequestDto input)
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

            var refills = await _fuelRefillRepository.GetAll()
                .Where(r => r.IdVehicle == input.IdVehicle)
                .Select(r=>ObjectMapper.Map<FuelRefillDto>(r))
                .ToListAsync();

            return new PagedResultEnumerableDto<FuelRefillDto>(refills, input).Get();
        }
        public async Task<long> InsertRefillAsync(InputRefillDto input)
        {
            var entity = ObjectMapper.Map<FuelRefill>(input);

            var result = await _fuelRefillRepository.InsertAndGetIdAsync(entity);

            return result;
        }

        public async Task DeleteRefillAsync(long idRefill)
        {
            await _fuelRefillRepository.DeleteAsync(x => x.Id == idRefill);
        }
    }
}