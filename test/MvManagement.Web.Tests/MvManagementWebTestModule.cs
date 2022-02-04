using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MvManagement.EntityFrameworkCore;
using MvManagement.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace MvManagement.Web.Tests
{
    [DependsOn(
        typeof(MvManagementWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class MvManagementWebTestModule : AbpModule
    {
        public MvManagementWebTestModule(MvManagementEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MvManagementWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(MvManagementWebMvcModule).Assembly);
        }
    }
}