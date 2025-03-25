using System.Net;
using System.Net.Mail;
using StudyFlow.Domain.Services.Email;

namespace StudyFlow.Infrastructure.Services.Email
{
    public class SendReviewReminderMail : ISendReviewReminderMail
    {
        private readonly string _credentialUser;
        private readonly string _credentialPassword;
        private readonly string _address;
        private readonly string _displayName;
        private readonly string _host;
        private readonly int _port;

        public SendReviewReminderMail(
            string credentialUser, 
            string credentialPassword,
            string address,
            string displayName,
            string host,
            int port)
        {
            _credentialUser = credentialUser;
            _credentialPassword = credentialPassword;
            _address = address;
            _displayName = displayName;
            _host = host;
            _port = port;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_credentialUser, _credentialPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(_address, _displayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await client.SendMailAsync(mailMessage);
        }
    }
}
