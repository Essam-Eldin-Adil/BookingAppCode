using Microsoft.AspNetCore.Http;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Services.Services
{
    public static class EmailSmtpService
    {

        public static bool SendEmail(HttpContext httpContext, string email, string body, string subject)
        {
            try
            {
                var emailSmtpInfo = GetEmailSmtpInfo(httpContext);
                var smtp = new SmtpClient(emailSmtpInfo.Host, emailSmtpInfo.Port);
                var basicAuthenticationInfo = new NetworkCredential(emailSmtpInfo.Email, emailSmtpInfo.Password);
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = basicAuthenticationInfo;
                smtp.EnableSsl = false;
                var mail = new MailMessage
                {
                    From = new MailAddress(emailSmtpInfo.Email)
                };
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static EmailSmtpInfo GetEmailSmtpInfo(HttpContext httpContext)
        {
            var emailSmtpInfo = new EmailSmtpInfo();
            var settingRepository = (IRepository<Data.Models.Setting>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Setting>));
            var settings = settingRepository.Table.ToList();

            emailSmtpInfo.Host = settings.FirstOrDefault(f => f.Key == "EmailHost")?.Value;

            if (settings.FirstOrDefault(f => f.Key == "EmailPort") != null)
                if(!string.IsNullOrEmpty(settings.FirstOrDefault(f => f.Key == "EmailPort").Value))
                emailSmtpInfo.Port = Convert.ToInt32(settings.FirstOrDefault(f => f.Key == "EmailPort").Value);

            emailSmtpInfo.Email = settings.FirstOrDefault(f => f.Key == "EmailUsername")?.Value;
            emailSmtpInfo.Password = settings.FirstOrDefault(f => f.Key == "EmailPassword")?.Value;
            return emailSmtpInfo;
        }


    }

}
