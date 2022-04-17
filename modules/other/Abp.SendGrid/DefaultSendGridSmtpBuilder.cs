using Abp.Dependency;
using SendGrid;

namespace Abp.SendGrid
{
    public class DefaultSendGridSmtpBuilder : ISendGridSmtpBuilder, ITransientDependency
    {
        private readonly IAbpSendGridConfiguration _abpMailKitConfiguration;

        public DefaultSendGridSmtpBuilder(IAbpSendGridConfiguration abpMailKitConfiguration)
        {
            _abpMailKitConfiguration = abpMailKitConfiguration;
        }

        public virtual SendGridClient Build()
        {
            var client = new SendGridClient(_abpMailKitConfiguration.ApiKey);

            try
            {
                ConfigureClient(client);
                return client;
            }
            catch
            {
                throw;
            }
        }

        protected virtual void ConfigureClient(SendGridClient client)
        {
           
        }
       
    }
}
