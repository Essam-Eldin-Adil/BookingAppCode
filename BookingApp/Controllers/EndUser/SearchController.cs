using Data.Models.General;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Models.Chalets.ChaletDetails;
using Data.ViewModels;
using iQuarc.DataLocalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Data.Models.Chalets;

namespace BookingApp.Controllers.EndUser
{
    public class SearchController : BaseController
    {
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<ParameterGroup> _groupRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<ChaletParameterValue> _chaletValuesRepository;

        public SearchController(IRepository<City> cityRepository,IRepository<Reservation> reservationRepository, IRepository<ParameterGroup> groupRepository, IRepository<Unit> unitRepository)
        {
            _cityRepository = cityRepository;
            _groupRepository = groupRepository;
            _unitRepository = unitRepository;
            _reservationRepository = reservationRepository;
        }

        public IActionResult Index(List<Guid> Cities,List<int> ProprtyType, string Date)
        {
            SearchViewModel model=new SearchViewModel();
            try
            {
                var cities = _cityRepository.Table.Select(c=>new
                {
                    c.CityName,
                    c.Id
                }).Localize(new CultureInfo(Lang)).ToList();

                foreach (var city in cities)
                {
                    model.Cities.Add(new City { CityName = city.CityName, Id = city.Id });
                }
                
                
                DataValidation(model, Date, Cities, ProprtyType);
                
                model.ParameterGroups = _groupRepository.Table.Include(c => c.Parameters).Where(c => c.Filterable).Localize(new CultureInfo(Lang)).ToList();

                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void DataValidation(SearchViewModel model, string Date, List<Guid> cities, List<int> ProprtyType)
        {
            if (cities.Count == 0)
                cities.Add(Guid.Empty);
            model.City = cities;
            if (ProprtyType.Count == 0)
                ProprtyType.Add((int)Enums.PropertyType.All);
            model.ProprtyType = ProprtyType;
            if (string.IsNullOrEmpty(Date))
            {
                model.DateFrom = DateTime.Now;
                model.DateTo = DateTime.Now.AddDays(1);
            }
            else
            {
                try
                {
                    var dates = Date.Split(" - ");
                    model.DateFrom = DateTime.Parse(dates[0]);
                    model.DateTo = DateTime.Parse(dates[1]);
                }
                catch
                {
                    model.DateFrom = DateTime.Now;
                    model.DateTo = DateTime.Now.AddDays(1);
                }
            }
        }

        [HttpGet]
        public IActionResult GetProperty(string PropertyName,string UnitCode, List<Guid> City, double PriceFrom, double PriceTo, string propType, string parameters,DateTime ArriveDate,DateTime LeaveDate, int Direction,string Neighborhood, int pageNumber = 1)
        {
            try
            {
                SearchItemViewModel searchItemViewModel = new SearchItemViewModel();
                var parms = new List<Guid>();
                var propTypes = new List<int>();
                if (!string.IsNullOrEmpty(parameters))
                {
                    parameters = parameters.Substring(0, parameters.Length - 1);
                    foreach (var item in parameters.Split(",").ToList())
                    {
                        parms.Add(Guid.Parse(item));
                    }
                }
                
                if (!string.IsNullOrEmpty(propType))
                {
                    propType = propType.Substring(0, propType.Length - 1);
                    foreach (var item in propType.Split(",").ToList())
                    {
                        propTypes.Add(int.Parse(item));
                    }
                }

                searchItemViewModel.PageSize = 10;
                var excludeRecord = (searchItemViewModel.PageSize * pageNumber) - searchItemViewModel.PageSize;
                searchItemViewModel.PageNumber = pageNumber;

                searchItemViewModel.Units = _unitRepository.Table.Include(c=>c.Offers).Include("UnitImages.File").Include(c=>c.Chalet.City)
                    .Where(c=>
                    c.ViewStatus&&c.Chalet.ViewStatus&&c.Chalet.IsConfirmed
                    &&(Direction==100|| c.Chalet.Direction==Direction)
                    &&(propType.Length==0|| propTypes.Contains(c.Chalet.PropertyType))
                    &&(c.DayPrice >= PriceFrom && c.DayPrice<= PriceTo)
                    &&(string.IsNullOrEmpty(UnitCode) ||c.Code== UnitCode)
                    &&(string.IsNullOrEmpty(PropertyName)||c.Name.Contains(PropertyName))
                    &&(!City.Any()||City.Contains(Guid.Empty) || City.Contains(c.Chalet.CityId))
                    &&(string.IsNullOrEmpty(Neighborhood) || c.Chalet.Neighborhood.Contains(Neighborhood))
                    &&(parms.Count==0 || c.ChaletParameterValues.Where(m=> parms.Contains(m.ParameterId)).Any())).Skip(excludeRecord).Take(searchItemViewModel.PageSize).ToList();

                foreach (var myUnit in searchItemViewModel.Units)
                {
                    myUnit.IsReserved = _reservationRepository.Any(c => c.UnitId == myUnit.Id
                    && (c.DateFrom.Date <= ArriveDate.Date && c.DateTo.Date >= ArriveDate.Date)
                    && (c.DateFrom.Date <= LeaveDate.Date && c.DateTo.Date >= LeaveDate.Date));
                    Domain.DateConverter.GetPricePerDay(HttpContext, myUnit, (int)ArriveDate.DayOfWeek);
                }

                searchItemViewModel.TotalRecord = _unitRepository.Table
                    .Count(c => (Direction == 100 || c.Chalet.Direction == Direction)
                    && (string.IsNullOrEmpty(UnitCode) || c.Code == UnitCode)
                    && (string.IsNullOrEmpty(PropertyName) || c.Name.Contains(PropertyName))
                    && (!City.Any() || City.Contains(Guid.Empty) || City.Contains(c.Chalet.CityId))
                    && (string.IsNullOrEmpty(Neighborhood) || c.Chalet.Neighborhood.Contains(Neighborhood)));
                return PartialView("_proprties", searchItemViewModel);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
