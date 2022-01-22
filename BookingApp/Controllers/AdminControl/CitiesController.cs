using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models.General;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace BookingApp.Controllers.AdminControl
{
    public class CitiesController : BaseController
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<CityTranslation> _cityTranslationRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<File> _fileRepository;

        public CitiesController(IRepository<City> cityRepository, IRepository<CityTranslation> cityTranslationRepository, IRepository<Country> countryTranslationRepository, IRepository<Country> countryRepository, IRepository<Language> languageRepository, IRepository<File> fileRepository)
        {
            _cityRepository = cityRepository;
            _cityTranslationRepository = cityTranslationRepository;
            _countryRepository = countryRepository;
            _languageRepository = languageRepository;
            _fileRepository = fileRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult<IEnumerable<City>> GetData()
        {
            try
            {
                var query = Request.Query;
                int take = 0;
                int skip = 0;
                int userType = 0;
                int.TryParse(query["length"], out take);
                int.TryParse(query["start"], out skip);
                var search = query["search[value]"][0];
                var draw = query["draw"];
                var data = _cityRepository.Table.Include(c=>c.Country).Where(c => (string.IsNullOrEmpty(search) || c.CityName == search)).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList();
                foreach (var city in data)
                {
                    city.ImageUrl = Url.Content("~/" + Domain.File.GetImage(HttpContext, city.Image));
                }
                return Ok(new { draw = draw, data, recordsTotal = _cityRepository.Table.Count(), recordsFiltered = _cityRepository.Table.Count() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult City(Guid? id)
        {
            var model = new CityViewModel();
            try
            {
                ViewBag.Countries = new SelectList(_countryRepository.Table.ToList(),"Id", "Name");
                ViewBag.Languages = new SelectList(_languageRepository.Table.ToList(),"Id", "Name");
                if (id!=null)
                {
                    model.City = _cityRepository.Table.Include("CityTranslations.Language").FirstOrDefault(c => c.Id == id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveCity(CityViewModel model)
        {
            try
            {
                var city = _cityRepository.Find(model.City.Id);
                if (city == null)
                {
                    city = new City {CityName = model.City.CityName, CountryId = model.City.CountryId};
                    _cityRepository.Add(city);
                }
                else
                {
                    city.CityName = model.City.CityName;
                    city.CountryId = model.City.CountryId;
                    _cityRepository.Update(city);
                }
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    if (city.Image != Guid.Empty&&city.Image!=null)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)city.Image);
                    }
                    var filesId = Domain.File.Upload("CitiesImages", _fileRepository, files);
                    foreach (var guid in filesId)
                    {
                        city.Image = guid;
                        _cityRepository.Update(city);
                    }
                }
                
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(City),new {id= city.Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        [HttpPost]
        public IActionResult SaveCityTranslation(CityViewModel model)
        {
            try
            {
                var city = _cityTranslationRepository.Find(model.CityTranslation.Id);
                if (city == null)
                {
                    _cityTranslationRepository.Add(model.CityTranslation);
                }
                else
                {
                    city.CityName = model.CityTranslation.CityName;
                    city.LanguageId = model.CityTranslation.LanguageId;
                    _cityTranslationRepository.Update(city);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(City), new { id = model.CityTranslation.CityId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IActionResult RemoveCity(Guid id)
        {
            try
            {
                var city = _cityRepository.Find(id);
                if (city != null)
                {
                    _cityRepository.Remove(city);
                    if (city.Image==Guid.Empty)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)city.Image);
                    }
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public IActionResult RemoveCityTranslation(Guid id)
        {
            try
            {
                var cityTranslation = _cityTranslationRepository.Find(id);
                if (cityTranslation != null)
                {
                    _cityTranslationRepository.Remove(cityTranslation);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
