using Data.Models.General;
using Domain;
using iQuarc.DataLocalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRepository<City> _cityRepository;
        public HomeController(IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public IActionResult Index()
        {
            //var doc = HtmlToPDF.HtmlToPdf("test");
            //System.IO.File.WriteAllBytes("hello.pdf", doc);
            var data = _cityRepository.Table.Select(c => new
            {
                c.CityName,
                c.Id,
                c.Image,
                c.ImageUrl
            }).Localize(new CultureInfo(Lang)).ToList();
            var cities = new List<City>();
            foreach (var city in data)
            {
                cities.Add(new City
                {
                    ImageUrl = Url.Content("~/" + Domain.File.GetImage(HttpContext, city.Image)),
                    Id = city.Id,
                    CityName = city.CityName,
                    Image = city.Image
                });
            }
            ViewBag.Cities = cities;
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
