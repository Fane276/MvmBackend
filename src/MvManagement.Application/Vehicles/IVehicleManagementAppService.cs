using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Vehicles
{
    public interface IVehicleManagementAppService
    {
        Task<PagedResultDto<VehicleDto>> GetCurrentUserPersonalVehiclesAsync(VehiclesPagedResultRequestDto input);
        Task<PagedResultDto<VehicleDto>> GetTenantVehiclesAsync(VehiclesPagedResultRequestDto input);
        Task<long> CreateVehicleAsync(VehicleDto input);
        Task UpdateVehicleAsync(VehicleDto input);
    }
}