using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Web.Mvc;

namespace ORUComSys.Extensions {
    public static class RazorViewToString {
        public static string Render(ControllerContext controllerContext, string viewPath, object model = null) {
            // First find the ViewEngine for this view
            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindView(controllerContext, viewPath, null);
            if(viewEngineResult == null) {
                throw new FileNotFoundException("View cannot be found.");
            }
            // Then get the view and attach the model to view data
            IView view = viewEngineResult.View;
            controllerContext.Controller.ViewData.Model = model;
            // Finally render the view and return it as a string
            string viewAsString = null;
            using(StringWriter sw = new StringWriter()) {
                ViewContext viewContext = new ViewContext(
                    controllerContext,
                    view,
                    controllerContext.Controller.ViewData,
                    controllerContext.Controller.TempData,
                    sw);
                view.Render(viewContext, sw);
                viewAsString = sw.ToString();
            }
            return viewAsString;
        }
    }
    public static class EmailSupport {
        public static void SendNotificationEmail(ControllerContext controllerContext, string viewPath, object model, string recipient, string subject) {
            MailAddress senderAddress = new MailAddress("do-not-reply@staggentreprises.com");
            MailAddress fromAddress = new MailAddress("do-not-reply@orucomsys.com");
            // Rendering the view to a string
            string requestBody = RazorViewToString.Render(controllerContext, viewPath, model);
            // Create the email
            MailMessage email = new MailMessage {
                Sender = senderAddress,
                From = fromAddress,
                Subject = subject,
                IsBodyHtml = true,
                Body = requestBody
            };
            // Add recipient
            email.To.Add(new MailAddress(recipient));
            // Send the email
            using(SmtpClient smtpClient = new SmtpClient()) {
                smtpClient.Send(email);
            }
        }
        public static void SendNotificationEmail(ControllerContext controllerContext, string viewPath, object model, List<string> recipients, string subject) {
            MailAddress senderAddress = new MailAddress("do-not-reply@staggentreprises.com");
            MailAddress fromAddress = new MailAddress("do-not-reply@orucomsys.com");
            // Rendering the view to a string
            string requestBody = RazorViewToString.Render(controllerContext, viewPath, model);
            // Create the email
            MailMessage email = new MailMessage {
                Sender = senderAddress,
                From = fromAddress,
                Subject = subject,
                IsBodyHtml = true,
                Body = requestBody
            };
            // Add recipient(s)
            foreach(string recipient in recipients) {
                email.To.Add(new MailAddress(recipient));
            }
            // Send the email
            using(SmtpClient smtpClient = new SmtpClient()) {
                smtpClient.Send(email);
            }
        }
    }
}