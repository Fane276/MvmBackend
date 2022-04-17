using Abp.Net.Mail;
using Abp.Threading;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace Abp.SendGrid
{
    public class SendGridEmailSender: EmailSenderBase
    {
        private readonly ISendGridSmtpBuilder _smtpBuilder;
        private readonly IEmailSenderConfiguration _smtpEmailSenderConfiguration;

        public SendGridEmailSender(
            IEmailSenderConfiguration smtpEmailSenderConfiguration,
            ISendGridSmtpBuilder smtpBuilder)
            : base(
                  smtpEmailSenderConfiguration)
        {
            _smtpBuilder = smtpBuilder;
            _smtpEmailSenderConfiguration = smtpEmailSenderConfiguration;
        }
        public override async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            var client = BuildSmtpClient();
            var message = BuildMimeMessage(from, to, subject, body, isBodyHtml);
            await client.SendEmailAsync(message);
        }

        public override void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            var client = BuildSmtpClient();
            var message = BuildMimeMessage(from, to, subject, body, isBodyHtml);
            AsyncHelper.RunSync(() => client.SendEmailAsync(message));
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            var client = BuildSmtpClient();
            var message = CreateFromMailMessage(mail);
            var response  = await client.SendEmailAsync(message).ConfigureAwait(false);
        }

        protected override void SendEmail(MailMessage mail)
        {
            var client = BuildSmtpClient();
            var message = CreateFromMailMessage(mail);
            AsyncHelper.RunSync(() => client.SendEmailAsync(message));
        }

        protected virtual SendGridClient BuildSmtpClient()
        {
            return _smtpBuilder.Build();
        }

        private SendGridMessage BuildMimeMessage(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            var message = new SendGridMessage();

            if (string.IsNullOrEmpty(from))
            {
                message.SetFrom(new EmailAddress(_smtpEmailSenderConfiguration.DefaultFromAddress, _smtpEmailSenderConfiguration.DefaultFromDisplayName));
            }

            message.SetSubject(subject);
            message.AddContent(isBodyHtml ? MimeType.Html : MimeType.Text, body);

            message.AddTo(new EmailAddress(to));

            return message;
        }


        private SendGridMessage CreateFromMailMessage(MailMessage mail)
        {
            var message = new SendGridMessage();

            if (string.IsNullOrEmpty(mail.From?.Address))
            {
                message.SetFrom(new EmailAddress(_smtpEmailSenderConfiguration.DefaultFromAddress, _smtpEmailSenderConfiguration.DefaultFromDisplayName));
            }
            else
            {
                message.SetFrom(new EmailAddress(_smtpEmailSenderConfiguration.DefaultFromAddress, mail.From?.DisplayName));
            }

            message.SetSubject(mail.Subject);
            message.AddContent(mail.IsBodyHtml ? MimeType.Html : MimeType.Text, mail.Body);

            message.AddTos(mail.To.Select(e=>new EmailAddress(e.Address,e.DisplayName)).ToList());

            return message;
        }

    }
}
