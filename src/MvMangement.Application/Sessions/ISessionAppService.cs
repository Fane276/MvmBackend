using System.Threading.Tasks;
using Abp.Application.Services;
using MvMangement.Sessions.Dto;

namespace MvMangement.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
