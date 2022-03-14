using Abp.Application.Services.Dto;

namespace MvManagement.Vehicles.Dto
{
    public class VehiclesPagedResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}