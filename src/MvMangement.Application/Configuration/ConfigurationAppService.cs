using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using MvMangement.Configuration.Dto;

namespace MvMangement.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : MvMangementAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
