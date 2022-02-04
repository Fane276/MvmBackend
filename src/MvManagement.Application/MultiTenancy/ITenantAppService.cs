using Abp.Application.Services;
using MvManagement.MultiTenancy.Dto;

namespace MvManagement.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

