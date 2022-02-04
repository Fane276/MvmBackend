using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MvManagement.Authorization;

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
    }
}
