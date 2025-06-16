using MimeKit;
using MailKit.Net.Smtp;

namespace FormsAPI.Services
{
    public class EmailService
    {
        private readonly string _verificationCode;

        public EmailService(IConfiguration configuration)
        {
            _verificationCode = configuration["EmailCode"]!;
        }

        public async Task SendAsync(string email, string subject, string message)
        {
            using var emailMessage = ConfigureEmailMessage(email, subject, message);
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    await client.AuthenticateAsync("7heproffi123@gmail.com", _verificationCode);
                    await client.SendAsync(emailMessage);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }

        public async Task<string> SendRecoveryCode(string email)
        {
            var code = GetRecoveryCode();
            await SendAsync(email, "Recovery code", $"Your recovery code: {code}");
            return code;
        }

        private MimeMessage ConfigureEmailMessage(string email, string subject, string message)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Администрация сайта FormsProject", "7heproffi123@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") {Text = message};
            return emailMessage;
        }

        private string GetRecoveryCode()
        {
            var code = new Random().Next(1000, 9999).ToString();
            return code;
        }
    }
}
