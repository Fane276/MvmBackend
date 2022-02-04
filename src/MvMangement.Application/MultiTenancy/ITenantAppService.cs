using Abp.Application.Services;
using MvMangement.MultiTenancy.Dto;

namespace MvMangement.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

