using Abp.Dependency;
using MvManagement.Configuration;
using MvManagement.Url;

namespace MvManagement.Web.Host.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
    }
}