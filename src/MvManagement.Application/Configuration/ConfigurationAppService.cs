using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MvManagement.Configuration.Dto;

namespace MvManagement.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MvManagementAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
