
using SendGrid;

namespace Abp.SendGrid
{
    public interface ISendGridSmtpBuilder
    {
        SendGridClient Build();
    }
}
