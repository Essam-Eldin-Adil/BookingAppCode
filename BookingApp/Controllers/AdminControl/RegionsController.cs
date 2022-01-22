using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models.Chalets;
using Data.Models.General;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace BookingApp.Controllers.AdminControl
{
    public class RegionsController : BaseController
    {
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<RegionTranslation> _regionTranslationRepository;
        private readonly IRepository<Language> _languageRepository;
        public RegionsController(IRepository<Region> regionRepository, IRepository<City> cityRepository, IRepository<RegionTranslation> regionTranslationRepository, IRepository<Language> languageRepository)
        {
            _regionRepository = regionRepository;
            _cityRepository = cityRepository;
            _regionTranslationRepository = regionTranslationRepository;
            _languageRepository = languageRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult<IEnumerable<Region>> GetData()
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
                var data = _regionRepository.Table.Include(c=>c.City).Where(c => (string.IsNullOrEmpty(search) || c.Name == search)).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList();
                return Ok(new { draw = draw, data, recordsTotal = _regionRepository.Table.Count(), recordsFiltered = _regionRepository.Table.Count() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult Region(Guid? id)
        {
            var model = new RegionViewModel();
            try
            {
                ViewBag.Cities = new SelectList(_cityRepository.Table.ToList(),"Id", "CityName");
                ViewBag.Languages = new SelectList(_languageRepository.Table.ToList(),"Id", "Name");
                if (id!=null)
                {
                    model.Region = _regionRepository.Table.Include("RegionTranslations.Language").FirstOrDefault(c => c.Id == id);
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
        public IActionResult SaveRegion(RegionViewModel model)
        {
            try
            {
                var region = _regionRepository.Find(model.Region.Id);
                if (region == null)
                {
                    region = new Region() {Name = model.Region.Name, CityId = model.Region.CityId};
                    _regionRepository.Add(region);
                }
                else
                {
                    region.Name = model.Region.Name;
                    region.CityId = model.Region.CityId;
                    _regionRepository.Update(region);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Region),new {id=model.Region.Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        [HttpPost]
        public IActionResult SaveRegionTranslation(RegionViewModel model)
        {
            try
            {
                var region = _regionTranslationRepository.Find(model.RegionTranslation.Id);
                if (region == null)
                {
                    _regionTranslationRepository.Add(model.RegionTranslation);
                }
                else
                {
                    region.Name = model.RegionTranslation.Name;
                    region.LanguageId = model.RegionTranslation.LanguageId;
                    _regionTranslationRepository.Update(region);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Region), new { id = model.RegionTranslation.RegionId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IActionResult RemoveRegion(Guid id)
        {
            try
            {
                var region = _regionRepository.Find(id);
                if (region != null)
                {
                    _regionRepository.Remove(region);
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

        public IActionResult RemoveRegionTranslation(Guid id)
        {
            try
            {
                var regionTranslation = _regionTranslationRepository.Find(id);
                if (regionTranslation != null)
                {
                    _regionTranslationRepository.Remove(regionTranslation);
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
