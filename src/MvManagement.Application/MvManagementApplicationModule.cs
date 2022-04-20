using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using MvManagement.Authorization;
using MvManagement.BackgroundWorkers;

namespace MvManagement
{
    [DependsOn(
        typeof(MvManagementCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MvManagementApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MvManagementAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MvManagementApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            // To enable auto-email sending uncomment this lines
            //var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //workManager.Add(IocManager.Resolve<ExpiringDocumentsEmailWorker>());
        }
    }
}
