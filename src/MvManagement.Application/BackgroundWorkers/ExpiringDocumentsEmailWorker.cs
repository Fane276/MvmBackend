using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Catalogue.Documents;
using Microsoft.EntityFrameworkCore;
using MvManagement.Authorization.Users;
using MvManagement.Documents.Dto;
using MvManagement.Documents.Insurance;
using MvManagement.Documents.PeriodicalDocuments;
using MvManagement.Documents.UserDocuments;
using MvManagement.Extensions;
using MvManagement.Net.Emailing;
using MvManagement.VehicleData;

namespace MvManagement.BackgroundWorkers
{
    public class ExpiringDocumentsEmailWorker : AsyncPeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<InsuranceDocument, long> _insuranceDocumentRepository;
        private readonly IRepository<Vehicle, long> _vehicleRepository;
        private readonly IRepository<PeriodicalDocument, long> _periodicalDocumentRepository;
        private readonly IRepository<PeriodicalDocumentType> _periodicalDocumentTypeRepository;
        private readonly IRepository<UserDocument, long> _userDocumentRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly UserManager _userManager;

        public ExpiringDocumentsEmailWorker(
            AbpAsyncTimer timer, 
            IRepository<InsuranceDocument, long> insuranceDocumentRepository,
            IRepository<Vehicle, long> vehicleRepository, 
            IRepository<PeriodicalDocument, long> periodicalDocumentRepository, 
            IRepository<PeriodicalDocumentType> periodicalDocumentTypeRepository, 
            IRepository<UserDocument, long> userDocumentRepository, 
            IEmailSender emailSender, 
            IEmailTemplateProvider emailTemplateProvider,
            UserManager userManager, 
            IRepository<User, long> userRepository) : base(timer)
        {
            _insuranceDocumentRepository = insuranceDocumentRepository;
            _vehicleRepository = vehicleRepository;
            _periodicalDocumentRepository = periodicalDocumentRepository;
            _periodicalDocumentTypeRepository = periodicalDocumentTypeRepository;
            _userDocumentRepository = userDocumentRepository;
            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
            _userManager = userManager;
            _userRepository = userRepository;

            LocalizationSourceName = "MvManagement";
            //Timer.Period = 5000; // 5 sec
            Timer.Period = 86400000; // 24hours
        }

        [UnitOfWork]
        protected override async Task DoWorkAsync()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var expiredDocuments = new List<EmailSendDocumentDto>();

                var isuranceExpired = await GetInsuranceForAllVehiclesAsync();
                expiredDocuments.AddRange(isuranceExpired);
                var periodicalExpired = await GetPeriodicalDocumentsForAllVehiclesAsync();
                expiredDocuments.AddRange(periodicalExpired);
                var userDocumentsExpired = await GetUsersDocumentsAsync();
                expiredDocuments.AddRange(userDocumentsExpired);

                foreach (var doc in expiredDocuments)
                {
                    await SendEmailAdaugareDocumentAsync(doc);
                }
            }
        }

        [UnitOfWork]
        private async Task<List<EmailSendDocumentDto>> GetInsuranceForAllVehiclesAsync()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var dateCompared = DateTime.Today.AddDays(5);
                var listOfDocuments = await (from document in _insuranceDocumentRepository.GetAll()
                        where document.ValidTo.CompareTo(dateCompared) < 0
                        join vehicle in _vehicleRepository.GetAll() on document.IdVehicle equals vehicle.Id
                        select new EmailSendDocumentDto
                        {
                            Name = document.InsuranceType.ToString(),
                            VehicleTitle = vehicle.Title,
                            ValidTo = document.ValidTo,
                            DocumentType = DocumentType.Insurance,
                            RegistrationNumber = vehicle.RegistrationNumber,
                            UserId = document.CreatorUserId ?? 0,
                            TenantId = vehicle.TenantId ?? 0,
                        })
                    .ToListAsync();
                return listOfDocuments;
            }
        }

        [UnitOfWork]
        private async Task<List<EmailSendDocumentDto>> GetPeriodicalDocumentsForAllVehiclesAsync()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var dateCompared = DateTime.Today.AddDays(5);
                var listOfDocuments = await (from document in _periodicalDocumentRepository.GetAll()
                        where document.ValidTo.CompareTo(dateCompared) < 0
                        join vehicle in _vehicleRepository.GetAll() on document.IdVehicle equals vehicle.Id
                        join documentType in _periodicalDocumentTypeRepository.GetAll() on document.IdPeriodicalDocumentType equals documentType.Id
                        select new EmailSendDocumentDto
                        {
                            Name = documentType.Name,
                            VehicleTitle = vehicle.Title,
                            ValidTo = document.ValidTo,
                            DocumentType = DocumentType.Periodical,
                            RegistrationNumber = vehicle.RegistrationNumber,
                            UserId = document.CreatorUserId ?? 0,
                            TenantId = vehicle.TenantId ?? 0,
                        })
                    .ToListAsync();
                return listOfDocuments;
            }
        }

        [UnitOfWork]
        private async Task<List<EmailSendDocumentDto>> GetUsersDocumentsAsync()
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var dateCompared = DateTime.Today.AddDays(5);
                var listOfDocuments = await (from document in _userDocumentRepository.GetAll()
                    where ((DateTime)document.ValidTo).CompareTo(dateCompared) < 0
                    join user in _userRepository.GetAll() on document.UserId equals user.Id
                    select new EmailSendDocumentDto
                    {
                        Name = document.DocumentType == UserDocumentType.OtherDocumentType ? document.OtherDocumentType : document.DocumentType.ToString(),
                        ValidTo = (DateTime)document.ValidTo,
                        DocumentType = DocumentType.UserDocument,
                        UserId = user.Id,
                        TenantId = user.TenantId ?? 0,
                    })
                .ToListAsync(); 
                return listOfDocuments;
            }
        }

        [UnitOfWork]
        private StringBuilder GetTitleAndSubTitle(int? tenantId, string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(tenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());
            await _emailSender.SendAsync(new MailMessage
            {
                To = { emailAddress },
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            });
        }
        [UnitOfWork]
        public virtual async Task SendEmailAdaugareDocumentAsync(EmailSendDocumentDto input)
        {
            var currentUser = await _userManager.GetUserOrNullAsync(new UserIdentifier(input.TenantId, input.UserId));

            var emailTemplate = GetTitleAndSubTitle(input.TenantId, $"{input.Name} is about to expire", $"Take into count to renew it until {input.ValidTo.ToString("d", new CultureInfo("ro"))}");
            var mailMessage = new StringBuilder();

            emailTemplate.Replace("{EMAIL_TITLE_ROW_STYLE}", "display: block; text-align: center;");
            emailTemplate.Replace("{EMAIL_SUB_TITLE_ROW_STYLE}", "display: block; text-align: center;");

            await ReplaceBodyAndSend(currentUser.EmailAddress, $"{input.Name} is about to expire", emailTemplate, mailMessage);
        }

    }
}