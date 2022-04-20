using System.Collections.Generic;
using Abp.Dependency;

namespace MvManagement.Url
{
    public interface IWebUrlService: ITransientDependency
    {
        string WebSiteRootAddressFormat { get; }

        string ServerRootAddressFormat { get; }

        bool SupportsTenancyNameInUrl { get; }

        string GetSiteRootAddress(string tenancyName = null);

        string GetServerRootAddress(string tenancyName = null);
        string GetTenantLogoUrl(int? tenantId = null);

        List<string> GetRedirectAllowedExternalWebSites();
    }
}