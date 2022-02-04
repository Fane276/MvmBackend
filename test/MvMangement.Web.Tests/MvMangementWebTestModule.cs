using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MvMangement.EntityFrameworkCore;
using MvMangement.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MvMangement.Web.Tests
{
    [DependsOn(
        typeof(MvMangementWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class MvMangementWebTestModule : AbpModule
    {
        public MvMangementWebTestModule(MvMangementEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MvMangementWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(MvMangementWebMvcModule).Assembly);
        }
    }
}