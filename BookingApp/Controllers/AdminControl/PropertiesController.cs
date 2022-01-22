using Data.Models.Chalets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers.AdminControl
{
    public class PropertiesController : BaseController
    {
        private readonly IRepository<Chalet> _chaletRepository;

        public PropertiesController(IRepository<Chalet> chaletRepository)
        {
            _chaletRepository = chaletRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult<IEnumerable<Chalet>> GetData()
        {
            try
            {
                var query = Request.Query;
                var take = 0;
                var skip = 0;
                int.TryParse(query["length"], out take);
                int.TryParse(query["start"], out skip);
                var search = query["search[value]"][0];
                var draw = query["draw"];
                var data = _chaletRepository.Table.Include(c=>c.City).Where(c => (string.IsNullOrEmpty(search) || c.ChaletName.Contains(search)) || (string.IsNullOrEmpty(search) || c.City.CityName.Contains(search) || (string.IsNullOrEmpty(search) || c.ChaletUsers.Select(c=>c.User.PhoneNumber).Contains(search)))).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList();
                return Ok(new { draw = draw, data, recordsTotal = _chaletRepository.Table.Count(), recordsFiltered = _chaletRepository.Table.Count() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //[HttpPost]
        public IActionResult ConfirmProperty(Guid id,bool confirm)
        {
            try
            {
                var chalet = _chaletRepository.Find(id);
                chalet.IsConfirmed = confirm;
                _chaletRepository.Update(chalet);
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
