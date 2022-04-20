using Microsoft.Extensions.Configuration;

namespace MvManagement.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}