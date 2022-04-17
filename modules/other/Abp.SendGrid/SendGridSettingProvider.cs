using Abp.Configuration;
using Abp.SendGrid.Constants;
using Abp.SendGrid.Extensions;

namespace Abp.SendGrid
{
    public class SendGridSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var group = new SettingDefinitionGroup(
              AbpSendGridModuleConsts.SendGridSettingGroup,
              AbpSendGridModuleConsts.SendGridSettingGroup.SendGridL());

            return new[] {
                new SettingDefinition(
                    AbpSendGridModuleConsts.SendGridSettingApiKey,
                    string.Empty,
                    AbpSendGridModuleConsts.SendGridSettingApiKey.SendGridL(),
                    group,
                    AbpSendGridModuleConsts.SendGridSettingApiKey.SendGridL(),
                    SettingScopes.Tenant,
                    isVisibleToClients:false)
            };
        }
    }
}
