using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Exchange.WebServices.Data;
using Services.Models;

namespace Services.Services
{
    public static class EmailExchangeService
    {
        public static bool SendMail(HttpContext httpContext, string email, string body, string subject)
        {
            try
            {
                var EmailExchangeInfo = GetEmailExchangeInfo(httpContext);
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
                service.Url = new Uri(EmailExchangeInfo.Url);
                service.Credentials = new NetworkCredential(EmailExchangeInfo.Email, EmailExchangeInfo.Password);

                EmailMessage message = new EmailMessage(service);
                message.ToRecipients.Add(email);
                message.Subject = subject;
                message.Body = new MessageBody(body);
                message.Body.BodyType = BodyType.HTML;
                message.SendAndSaveCopy();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static EmailExchangeInfo GetEmailExchangeInfo(HttpContext httpContext)
        {
            var emailExchangeInfo = new EmailExchangeInfo();
            var settingRepository = (IRepository<Data.Models.Setting>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Setting>));
            var settings = settingRepository.Table.ToList();

            emailExchangeInfo.Url = settings.FirstOrDefault(f => f.Key == "EmailHost")?.Value;
            emailExchangeInfo.Email = settings.FirstOrDefault(f => f.Key == "EmailUsername")?.Value;
            emailExchangeInfo.Password = settings.FirstOrDefault(f => f.Key == "EmailPassword")?.Value;
            return emailExchangeInfo;
        }
    }
}
