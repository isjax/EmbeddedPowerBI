using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbeddedPowerBI.Services.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings settings;

        public EmailSender(IOptions<SmtpSettings> settings)
        {
            this.settings = settings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();

            message.To.Add(new MailboxAddress(email));
            //message.Cc.Add(new MailboxAddress(settings.SmtpUsername));
            message.From.Add(new MailboxAddress(settings.SmtpUsername));

            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlMessage
            };

            return SmtpSendAsync(message);
        }

        private async Task SmtpSendAsync(MimeMessage message)
        {
            using (MailKit.Net.Smtp.SmtpClient emailClient = new MailKit.Net.Smtp.SmtpClient())
            {
                await emailClient.ConnectAsync(settings.SmtpServer, settings.SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.AuthenticateAsync(settings.SmtpUsername, settings.SmtpPassword);
                await emailClient.SendAsync(message);
                await emailClient.DisconnectAsync(true);
            }
        }
    }

}

