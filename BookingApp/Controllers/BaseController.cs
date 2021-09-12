using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace BookingApp.Controllers
{
    public class BaseController :Controller
    {
        public string IdentityId { get; set; }
        public string IdentityImage { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (HttpContext.Session.GetString("UserId") != null)
            //{
            //    IdentityId = HttpContext.Session.GetString("UserId");
            //}

            //if (HttpContext.Session.GetString("UserImage") != null)
            //{
            //    IdentityImage = HttpContext.Session.GetString("UserImage");
            //}

            if (HttpContext.Session.GetString("Language") == null)
            {
                Language("ar-EG", "flag-icon-sa", Resource.Arabic, "RTL");
            }


            Thread.CurrentThread.CurrentCulture = new CultureInfo(HttpContext.Session.GetString("Language"));
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
        public void Language(string code, string flag, string name, string direction)
        {
            if (string.IsNullOrEmpty(code)) return;
            HttpContext.Session.SetString("Language", code);
            HttpContext.Session.SetString("LanguageFlag", flag);
            HttpContext.Session.SetString("LanguageName", name);
            HttpContext.Session.SetString("LanguageDirection", direction);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(HttpContext.Session.GetString("Language"));
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
        public void SavedSuccess()
        {
            TempData["AlertType"] = "success";
            TempData["AlertTitle"] = Resource.AlertDataSavedSuccessfully;
            TempData["AlertMessage"] = Resource.SystemAlert;
        }

        public void Success(string Message, string title = "")
        {
            TempData["AlertType"] = "success";
            TempData["AlertTitle"] = title;
            TempData["AlertMessage"] = Message;
        }
        public void Error(string Message, string title = "")
        {
            TempData["AlertType"] = "error";
            TempData["AlertTitle"] = title;
            TempData["AlertMessage"] = Message;
        }
        public void Info(string Message, string title = "")
        {
            TempData["AlertType"] = "info";
            TempData["AlertTitle"] = title;
            TempData["AlertMessage"] = Message;
        }
        public void Warning(string Message, string title = "")
        {
            TempData["AlertType"] = "warning";
            TempData["AlertTitle"] = title;
            TempData["AlertMessage"] = Message;
        }

        

    }
}