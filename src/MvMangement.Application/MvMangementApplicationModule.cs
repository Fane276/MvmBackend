using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MvMangement.Authorization;

namespace MvMangement
{
    [DependsOn(
        typeof(MvMangementCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MvMangementApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MvMangementAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MvMangementApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
