using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Vehicles
{
    public interface IVehicleManagementAppService
    {
        Task<PagedResultDto<VehicleDto>> GetCurrentUserPersonalVehiclesAsync(PagedSortedAndFilteredInputDto input);
        Task<PagedResultDto<VehicleDto>> GetTenantVehiclesAsync(PagedSortedAndFilteredInputDto input);
        Task<long> CreateVehicleAsync(VehicleDto input);
        Task UpdateVehicleAsync(VehicleDto input);
        Task DeleteVehicleAsync(long idVehicle);
    }
}