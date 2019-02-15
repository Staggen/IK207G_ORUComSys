using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;

namespace ORUComSys.Controllers {
    public class EmailController : Controller {
        // Mail settings
        private SmtpClient smtpClient;
        private MailAddress senderAddress;
        private MailAddress mailFrom;

        public EmailController() {
            // Mail settings
            smtpClient = new SmtpClient();
            senderAddress = new MailAddress("do-not-reply@staggentreprises.com");
            mailFrom = new MailAddress("do-not-reply@orucomsys.com");
        }

        public void SendEmail(string recipient, string subject, string requestBody) {
            // Create the email
            MailMessage email = new MailMessage {
                Sender = senderAddress,
                From = mailFrom,
                Subject = subject,
                IsBodyHtml = true,
                Body = requestBody
            };
            // Add recipient
            email.To.Add(new MailAddress(recipient));
            // Send the email
            smtpClient.Send(email);
        }

        public void SendEmail(List<string> recipientList, string subject, string requestBody) {
            // Create the email
            MailMessage email = new MailMessage {
                Sender = senderAddress,
                From = mailFrom,
                Subject = subject,
                IsBodyHtml = true,
                Body = requestBody
            };
            // Add recipient(s)
            foreach(var recipient in recipientList) {
                email.To.Add(new MailAddress(recipient));
            }
            // Send the email
            smtpClient.Send(email);
        }
    }
}