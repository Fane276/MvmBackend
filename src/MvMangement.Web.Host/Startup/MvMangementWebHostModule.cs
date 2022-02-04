using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MvMangement.Configuration;

namespace MvMangement.Web.Host.Startup
{
    [DependsOn(
       typeof(MvMangementWebCoreModule))]
    public class MvMangementWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MvMangementWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MvMangementWebHostModule).GetAssembly());
        }
    }
}
