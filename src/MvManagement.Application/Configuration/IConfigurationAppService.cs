using System.Threading.Tasks;
using MvManagement.Configuration.Dto;

namespace MvManagement.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
