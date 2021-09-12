using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangeLanguage(string code, string flag, string name, string direction)
        {
            base.Language(code, flag, name, direction);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
