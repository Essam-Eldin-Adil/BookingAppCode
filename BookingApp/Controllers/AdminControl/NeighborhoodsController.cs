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
    public class NeighborhoodsController : BaseController
    {
        private readonly IRepository<Neighborhood> _neighborhoodRepository;
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<NeighborhoodTranslation> _regionTranslationRepository;
        private readonly IRepository<Language> _languageRepository;
        public NeighborhoodsController(IRepository<Neighborhood> neighborhoodRepository, IRepository<Region> cityRepository, IRepository<NeighborhoodTranslation> regionTranslationRepository, IRepository<Language> languageRepository)
        {
            _neighborhoodRepository = neighborhoodRepository;
            _regionRepository = cityRepository;
            _regionTranslationRepository = regionTranslationRepository;
            _languageRepository = languageRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult<IEnumerable<Neighborhood>> GetData()
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
                var data = _neighborhoodRepository.Table.Include(c=>c.Region).Where(c => (string.IsNullOrEmpty(search) || c.Name == search)).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList();
                return Ok(new { draw = draw, data, recordsTotal = _neighborhoodRepository.Table.Count(), recordsFiltered = _neighborhoodRepository.Table.Count() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult Neighborhood(Guid? id)
        {
            var model = new NeighborhoodViewModel();
            try
            {
                ViewBag.Regions = new SelectList(_regionRepository.Table.ToList(),"Id", "Name");
                ViewBag.Languages = new SelectList(_languageRepository.Table.ToList(),"Id", "Name");
                if (id!=null)
                {
                    model.Neighborhood = _neighborhoodRepository.Table.Include("NeighborhoodTranslations.Language").FirstOrDefault(c => c.Id == id);
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
        public IActionResult SaveNeighborhood(NeighborhoodViewModel model)
        {
            try
            {
                var neighborhood = _neighborhoodRepository.Find(model.Neighborhood.Id);
                if (neighborhood == null)
                {
                    neighborhood = new Neighborhood() {Name = model.Neighborhood.Name, RegionId = model.Neighborhood.RegionId };
                    _neighborhoodRepository.Add(neighborhood);
                }
                else
                {
                    neighborhood.Name = model.Neighborhood.Name;
                    neighborhood.RegionId = model.Neighborhood.RegionId;
                    _neighborhoodRepository.Update(neighborhood);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Neighborhood),new {id=model.Neighborhood.Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        [HttpPost]
        public IActionResult SaveNeighborhoodTranslation(NeighborhoodViewModel model)
        {
            try
            {
                var neighborhood = _regionTranslationRepository.Find(model.NeighborhoodTranslation.Id);
                if (neighborhood == null)
                {
                    _regionTranslationRepository.Add(model.NeighborhoodTranslation);
                }
                else
                {
                    neighborhood.Name = model.NeighborhoodTranslation.Name;
                    neighborhood.LanguageId = model.NeighborhoodTranslation.LanguageId;
                    _regionTranslationRepository.Update(neighborhood);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Neighborhood), new { id = model.NeighborhoodTranslation.NeighborhoodId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IActionResult RemoveNeighborhood(Guid id)
        {
            try
            {
                var neighborhood = _neighborhoodRepository.Find(id);
                if (neighborhood != null)
                {
                    _neighborhoodRepository.Remove(neighborhood);
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

        public IActionResult RemoveNeighborhoodTranslation(Guid id)
        {
            try
            {
                var neighborhoodTranslation = _regionTranslationRepository.Find(id);
                if (neighborhoodTranslation != null)
                {
                    _regionTranslationRepository.Remove(neighborhoodTranslation);
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
