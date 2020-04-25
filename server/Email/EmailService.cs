using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using server.Configuration;

namespace server.Email
{
    public class EmailService
    {
        private ISendGridClient email;
        private EmailSettings emailSettings;

        public EmailService(ISendGridClient email, EmailSettings emailSettings)
        {
            this.email = email;
            this.emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(string userName, string message)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailSettings.FromEmail),
                Subject = emailSettings.Subject,
                PlainTextContent = $"{message} - from {userName}"
            };

            msg.AddTo(new EmailAddress(emailSettings.ToEmail));
            await email.SendEmailAsync(msg);
        }
    }
}