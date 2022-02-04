using System.Threading.Tasks;
using Abp.Application.Services;
using MvMangement.Authorization.Accounts.Dto;

namespace MvMangement.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
