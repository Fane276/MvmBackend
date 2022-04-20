using System.Collections.Generic;
using System.Linq;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using MvManagement.Configuration;

namespace MvManagement.Url
{
    public abstract class WebUrlServiceBase
    {
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        public abstract string WebSiteRootAddressFormatKey { get; }

        public abstract string ServerRootAddressFormatKey { get; }

        public string WebSiteRootAddressFormat => _appConfiguration[WebSiteRootAddressFormatKey] ?? "http://localhost:62114/";

        public string ServerRootAddressFormat => _appConfiguration[ServerRootAddressFormatKey] ?? "http://localhost:62114/";

        public bool SupportsTenancyNameInUrl
        {
            get
            {
                var siteRootFormat = WebSiteRootAddressFormat;
                return !siteRootFormat.IsNullOrEmpty() && siteRootFormat.Contains(TenancyNamePlaceHolder);
            }
        }

        private readonly IConfigurationRoot _appConfiguration;

        protected WebUrlServiceBase(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }

        public string GetSiteRootAddress(string tenancyName = null)
        {
            return WebSiteRootAddressFormat;
        }

        public string GetServerRootAddress(string tenancyName = null)
        {
            return ServerRootAddressFormat;
        }

        public List<string> GetRedirectAllowedExternalWebSites()
        {
            var values = _appConfiguration["App:RedirectAllowedExternalWebSites"];
            return values?.Split(',').ToList() ?? new List<string>();
        }


        public string GetTenantLogoUrl(int? tenantId)
        {
            if (!tenantId.HasValue)
            {
                return GetServerRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo";
            }

            return GetServerRootAddress().EnsureEndsWith('/') + "TenantCustomization/GetTenantLogo?tenantId=" + tenantId.Value;
        }
    }
}