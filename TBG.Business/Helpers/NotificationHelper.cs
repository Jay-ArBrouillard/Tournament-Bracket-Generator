using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TBG.Business.Helpers
{
    public static class NotificationHelper
    {
        public static async Task sendEmail(string toAddr, string toName, string subject, string body)
        {
            AppSettingsReader settingsReader = new AppSettingsReader();

            var fromAddr = (string)settingsReader.GetValue("EmailFrom", typeof(String));
            var emailPassword = (string)settingsReader.GetValue("EmailPassword", typeof(String));
            var displayname = (string)settingsReader.GetValue("EmailDisplayName", typeof(String));

            var fromAddress = new MailAddress(fromAddr, displayname);
            var toAddress = new MailAddress(toAddr, toName);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, emailPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }
    }
}