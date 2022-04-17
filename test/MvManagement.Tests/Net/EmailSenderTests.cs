using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using Xunit;

namespace MvManagement.Tests.Net
{
    public class EmailSenderTests : MvManagementTestBase
    {
        private readonly IEmailSender _emailSender;

        public EmailSenderTests()
        {
            _emailSender = Resolve<IEmailSender>();
        }

        [Fact]
        public async Task TrimiteEmailAsync()
        {
            var msg = new MailMessage
            {
                To = { "stefan.sc.sc@gmail.com" },
                From = new MailAddress("stefan.cirstea@yohomail.com"),
                Subject = "MvManagement Mesaj test",
                Body = "Facem caateva teste sa vedem de ce nu ati primit email-ul. V-a rugam sa ignorati acest email. Multumim",
                IsBodyHtml = true
            };
            await _emailSender.SendAsync(msg);
        }
        [Fact]
        public async Task IntegrationSendgridAsync()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("stefan.cirstea@zohomail.eu", "MvManagement");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("cirsteastefan0027@yahoo.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}