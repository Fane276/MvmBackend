using System.Threading.Tasks;
using Abp.Application.Services;
using MvManagement.Sessions.Dto;

namespace MvManagement.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
