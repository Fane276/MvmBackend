using System;
using Castle.MicroKernel.Registration;
using NSubstitute;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Configuration.Startup;
using Abp.Net.Mail;
using Abp.SendGrid;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Abp.Zero.EntityFrameworkCore;
using MvManagement.EntityFrameworkCore;
using MvManagement.Tests.DependencyInjection;

namespace MvManagement.Tests
{
    [DependsOn(
        typeof(MvManagementApplicationModule),
        typeof(MvManagementEntityFrameworkModule),
        typeof(AbpTestBaseModule)
        )]
    public class MvManagementTestModule : AbpModule
    {
        public MvManagementTestModule(MvManagementEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator<MvManagementDbContext>>();


            Configuration.Modules.AbpSendGrid().ApiKey = MvManagementConsts.SendGridApiKey;

            Configuration.ReplaceService<ISendGridSmtpBuilder, DefaultSendGridSmtpBuilder>(DependencyLifeStyle.Transient);
            Configuration.ReplaceService<IEmailSender, SendGridEmailSender>(DependencyLifeStyle.Transient);
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>() where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );
        }
    }
}
