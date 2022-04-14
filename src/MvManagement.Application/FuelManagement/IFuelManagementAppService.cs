using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Chart;
using MvManagement.FuelManagement.Dto;
using MvManagement.Vehicles.Dto;

namespace MvManagement.FuelManagement
{
    public interface IFuelManagementAppService
    {
        Task<PagedResultDto<FuelRefillDto>> GetVehicleRefillsAsync(VehiclesPagedResultRequestDto input);
        Task<long> InsertRefillAsync(InputRefillDto input);
        Task DeleteRefillAsync(long idRefill);
        Task<BarChart> GetPricePerLastWeekAsync(long idVehicle, int numberDays = 7);
    }
}