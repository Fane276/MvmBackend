using System;
using System.Collections.Concurrent;
using System.Text;
using Abp.Dependency;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Reflection.Extensions;
using MvManagement.Url;

namespace MvManagement.Net.Emailing
{
    public class EmailTemplateProvider : IEmailTemplateProvider, ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, string> _defaultTemplates;
        private readonly IWebUrlService _webUrlService;

        public EmailTemplateProvider(IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
            _defaultTemplates = new ConcurrentDictionary<string, string>();
        }

        public string GetDefaultTemplate(int? tenantId)
        {
            var tenancyKey = tenantId.HasValue ? tenantId.Value.ToString() : "host";

            return _defaultTemplates.GetOrAdd(tenancyKey, key =>
            {
                using (var stream = typeof(EmailTemplateProvider).GetAssembly().GetManifestResourceStream("MvManagement.Net.Emailing.EmailTemplates.default.html"))
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
                    template = template.Replace("{THIS_YEAR}", DateTime.Now.Year.ToString());
                    return template.Replace("{EMAIL_LOGO_URL}", _webUrlService.GetServerRootAddress().EnsureEndsWith('/') + "mvm-logo-large.png");
                }
            });
        }
    }
}