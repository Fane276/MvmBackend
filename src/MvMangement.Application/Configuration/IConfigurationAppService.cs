using System.Threading.Tasks;
using MvMangement.Configuration.Dto;

namespace MvMangement.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
