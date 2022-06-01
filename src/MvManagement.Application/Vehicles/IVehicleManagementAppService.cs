using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Extensions.Dto.PageFilter;
using MvManagement.Vehicles.Dto;

namespace MvManagement.Vehicles
{
    public interface IVehicleManagementAppService
    {
        /// <summary>
        /// Retrieve the vehicles of the current user
        /// </summary>
        Task<PagedResultDto<VehicleDto>> GetCurrentUserPersonalVehiclesAsync(PagedSortedAndFilteredInputDto input);
        Task<PagedResultDto<VehicleDto>> GetTenantVehiclesAsync(PagedSortedAndFilteredInputDto input);
        /// <summary>
        /// Create new vehicle, if tenantId is specified than the ownership is on tenant, if not the ownership is on user
        /// Chassis number should be exact 17 characters long
        /// Vehicle production year should be a number between 1886 and current year
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Id of the new inserted vehicle</returns>
        Task<long> CreateVehicleAsync(VehicleCreateDto input);
        /// <summary>
        /// Update vehicle, if tenantId is specified than the ownership is on tenant, if not the ownership is on user
        /// Chassis number should be exact 17 characters long
        /// Vehicle production year should be a number between 1886 and current year
        /// </summary>
        /// <param name="input"></param>
        Task UpdateVehicleAsync(VehicleCreateDto input);
        /// <summary>
        /// Delete the vehicle with specified id
        /// </summary>
        Task DeleteVehicleAsync(long idVehicle);

        Task<VehicleDto> GetVehicleByIdAsync(long idVehicle);
    }
}