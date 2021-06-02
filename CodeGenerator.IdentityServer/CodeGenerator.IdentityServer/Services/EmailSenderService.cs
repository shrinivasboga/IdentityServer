using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Brolink.IdentityServer.Services
{
    public class EmailSenderService : IEmailSender
    {
        public EmailSenderService()
        {

        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
