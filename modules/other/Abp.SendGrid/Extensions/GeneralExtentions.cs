using Abp.Localization;
using Abp.SendGrid.Constants;

namespace Abp.SendGrid.Extensions
{
    public static class GeneralExtentions
    {
        public static ILocalizableString SendGridL(this string name)
        {
            return new LocalizableString(name, AbpSendGridModuleConsts.LocalizationSourceName);
        }
    }
}
