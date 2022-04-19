using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Net.Mail;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using MvManagement.Documents.Dto;
using MvManagement.Documents.Insurance.Dto;
using MvManagement.Net.Emailing;
using MvManagement.VehicleData;
using MvManagement.Vehicles;

namespace MvManagement.Documents.Insurance
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class InsuranceAppService : VehicleAppServiceBase, IInsuranceAppService
    {
        private readonly IRepository<InsuranceDocument, long> _insuranceDocumentRepository;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<Vehicle, long> _vehicleRepository;

        public InsuranceAppService(
            IVehiclePermissionManager vehiclePermissionManager, 
            IRepository<InsuranceDocument, long> insuranceDocumentRepository, 
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender, 
            IRepository<Vehicle, long> vehicleRepository) : base(vehiclePermissionManager)
        {
            _insuranceDocumentRepository = insuranceDocumentRepository;
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
            _vehicleRepository = vehicleRepository;
        }
        public async Task SaveInsuranceAsync(InsuranceDocumentDto document)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(document.IdVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.Edit} is missing");
            }

            var insurance = await _insuranceDocumentRepository.GetAll()
                .FirstOrDefaultAsync(d => d.IdVehicle == document.IdVehicle && d.InsuranceType == document.InsuranceType);
            if (insurance != null)
            {
                throw new UserFriendlyException($"Already has an insurance of type {document.InsuranceType}");
            }

            var entity = ObjectMapper.Map<InsuranceDocument>(document);

            await _insuranceDocumentRepository.InsertAsync(entity);

            //await SendEmailAdaugareDocumentAsync("rca");
        }

        public async Task<InsuranceResultDto> GetInsurancesForVehicleAsync(long idVehicle)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(idVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.View);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.View} is missing");
            }

            var rcaInsurance = await _insuranceDocumentRepository.GetAll()
                .Where(d => d.IdVehicle == idVehicle && d.InsuranceType == InsuranceType.Rca)
                .Select(d => ObjectMapper.Map<InsuranceDocumentDto>(d))
                .FirstOrDefaultAsync();

            var cascoInsurance = await _insuranceDocumentRepository.GetAll()
                .Where(d => d.IdVehicle == idVehicle && d.InsuranceType == InsuranceType.Casco)
                .Select(d => ObjectMapper.Map<InsuranceDocumentDto>(d))
                .FirstOrDefaultAsync();

            var insuranceResult = new InsuranceResultDto()
            {
                Rca = rcaInsurance,
                Casco = cascoInsurance
            };

            return insuranceResult;
        }

        public async Task<InsuranceIdsResultDto> GetInsuranceIdsForVehicleAsync(long idVehicle)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(idVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.View);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.View} is missing");
            }

            var rcaInsurance = await _insuranceDocumentRepository.GetAll()
                .Where(d => d.IdVehicle == idVehicle && d.InsuranceType == InsuranceType.Rca)
                .Select(d => ObjectMapper.Map<InsuranceDocumentDto>(d))
                .FirstOrDefaultAsync();

            var cascoInsurance = await _insuranceDocumentRepository.GetAll()
                .Where(d => d.IdVehicle == idVehicle && d.InsuranceType == InsuranceType.Casco)
                .Select(d => ObjectMapper.Map<InsuranceDocumentDto>(d))
                .FirstOrDefaultAsync();

            var insuranceResult = new InsuranceIdsResultDto()
            {
                RcaId = rcaInsurance?.Id,
                CascoId = cascoInsurance?.Id
            };

            return insuranceResult;
        }

        public async Task DeleteInsurance(long idInsurance)
        {
            await _insuranceDocumentRepository.DeleteAsync(d => d.Id == idInsurance);
        }

        public async Task EditInsuranceAsync(InsuranceDocumentDto document)
        {
            var hasPermission = await VehiclePermissionManager.CheckCurrentUserPermissionAsync(document.IdVehicle, VehiclePermissionNames.VehicleDocuments.Insurance.Edit);

            if (!hasPermission)
            {
                throw new UserFriendlyException($"Not authorized, {VehiclePermissionNames.VehicleDocuments.Insurance.Edit} is missing");
            }

            var entity = ObjectMapper.Map<InsuranceDocument>(document);

            await _insuranceDocumentRepository.UpdateAsync(entity);
        }

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
        public virtual async Task SendEmailAdaugareDocumentAsync(string denumire)
        {
            if (AbpSession.UserId == null)
            {
                throw new AbpAuthorizationException("Not authorized");
            }
            var currentUser =await UserManager.GetUserOrNullAsync(new UserIdentifier(AbpSession.TenantId, (long)AbpSession.UserId));

            var emailTemplate = GetTitleAndSubTitle(AbpSession.TenantId, "Document added", denumire);
            var mailMessage = new StringBuilder();
            var culoareStatus = "#28a745";

            emailTemplate.Replace("{EMAIL_TITLE_ROW_STYLE}", "display:none;");
            emailTemplate.Replace("{EMAIL_SUB_TITLE_ROW_STYLE}", "display:none;");

            mailMessage.AppendLine(
                $"<br /><div style='width: 100vw; max-width:680px;'><span style='float:left; margin-left:25px;'>{L("StatusulProiectului")}: <span style='height: 10px;width: 10px;border-radius: 50%;display: inline-block;background-color: {culoareStatus};'></span> <b>Ceva</b></span></div>");
            
            await ReplaceBodyAndSend(currentUser.EmailAddress, $"{L("ProiectDepus")}", emailTemplate, mailMessage);
        }

        public async Task<List<ExpiredDocumentDto>> GetExpiredInsuranceForAllUserVehiclesAsync()
        {
            var currentUserId = AbpSession.UserId;

            if (currentUserId == null)
            {
                throw new AbpAuthorizationException("User not authenticated");
            }

            var vehcicleIds = await _vehicleRepository.GetAll()
                .Where(vehicle => vehicle.UserId == currentUserId)
                .Select(vehicle => vehicle.Id).ToListAsync();

            var listOfDocuments = await (from document in _insuranceDocumentRepository.GetAll()
                where vehcicleIds.Contains(document.IdVehicle) && document.ValidTo.CompareTo(DateTime.Today) < 0
                join vehicle in _vehicleRepository.GetAll() on document.IdVehicle equals vehicle.Id
                select new ExpiredDocumentDto
                {
                    Id = document.Id,
                    Name = document.InsuranceType.ToString(),
                    VehicleTitle = vehicle.Title,
                    ValidTo = document.ValidTo,
                    DocumentType = DocumentType.Insurance
                })
            .ToListAsync();

            return listOfDocuments;
        }
    }
}