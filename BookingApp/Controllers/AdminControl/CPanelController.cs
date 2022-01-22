using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers.AdminControl
{
    public class CPanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
