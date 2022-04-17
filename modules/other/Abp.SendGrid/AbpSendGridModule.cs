using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Reflection.Extensions;
using Abp.SendGrid.Constants;
using System.Reflection;

namespace Abp.SendGrid
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpSendGridModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IAbpSendGridConfiguration, AbpSendGridConfiguration>();
            Configuration.ReplaceService<IEmailSender, SendGridEmailSender>(DependencyLifeStyle.Transient);


            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpSendGridModuleConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(),
                        "Abp.SendGrid.Localization.Source"
                    )
                )
            );

            Configuration.Settings.Providers.Add<SendGridSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpSendGridModule).GetAssembly());
        }
    }
}
