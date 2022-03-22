using Abp.Application.Services.Dto;
using MvManagement.Extensions.Dto.PageFilter;

namespace MvManagement.Vehicles.Dto
{
    public class VehiclesPagedResultRequestDto : PagedSortedAndFilteredInputDto
    {
        public long IdVehicle { get; set; }
    }
}