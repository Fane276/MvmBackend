using Abp.Configuration.Startup;

namespace Abp.SendGrid
{
    public static class AbpSendGridConfigurationExtensions
    {
        public static IAbpSendGridConfiguration AbpSendGrid(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpSendGridConfiguration>();
        }
    }
}
