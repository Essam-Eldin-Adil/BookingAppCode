using Data.Models;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.Chalets.RatingAndReview;
using Data.Models.General;
using Data.ViewModels;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BookingApp.Authrize;

namespace BookingApp.Controllers.EndUser
{
    public class UnitController : BaseController
    {
        private readonly IRepository<Unit> _unitRpository;
        private readonly IRepository<ParameterGroup> _parameterGroupRpository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Fiverate> _fiveRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Rate> _rateRepository;
        private readonly IRepository<City> _cityRepository;
private readonly IRepository<PricePerDay> _pricePerDayRepository;
private readonly IRepository<Offer> _offersRepository;
        private readonly IRepository<Reservation> _reservationRepository;
public UnitController(IRepository<Unit> unitRpository, IRepository<ParameterGroup> parameterGroupRpository, IRepository<User> userRepository,
            IRepository<Fiverate> fiveRepository, IRepository<Rate> rateRepository, IRepository<PricePerDay> pricePerDayRepository,
            IRepository<Unit> unitRepository,
            IRepository<Offer> offersRepository,
            IRepository<Reservation> reservationRepository,
        IRepository<City> cityRepository)
        {
            _unitRpository = unitRpository;
            _parameterGroupRpository = parameterGroupRpository;
            _userRepository = userRepository;
            _fiveRepository = fiveRepository;
            _rateRepository = rateRepository;
            _unitRepository = unitRepository;
            _cityRepository = cityRepository;
            _offersRepository = offersRepository;
            _pricePerDayRepository = pricePerDayRepository;
            _reservationRepository = reservationRepository;
        }

        // GET: Unit
        public ActionResult Index(Guid id, DateTime DateFrom, DateTime DateTo)
        {
            SingleUnitViewModel model = new SingleUnitViewModel();
            try
            {

                model.Unit = _unitRpository.Table
                    .Include("ChaletParameterValues.Parameter.ParameterGroup")
                    .Include("UnitImages.File")
                    .Include(c => c.Chalet.City)
                    .Include("Chalet.ResortParameterValue.Parameter.ParameterGroup")
                    .Include(c=>c.Offers)
                    .FirstOrDefault(c => c.Id == id);
                Domain.DateConverter.GetPricePerDay(HttpContext, model.Unit, (int)DateFrom.DayOfWeek);

                model.Unit.IsReserved= _reservationRepository.Any(c => c.UnitId == model.Unit.Id 
                && (c.DateFrom.Date <= DateFrom.Date && c.DateTo.Date >= DateFrom.Date)
                && (c.DateFrom.Date <= DateTo.Date && c.DateTo.Date >= DateTo.Date));
                model.Unit.Views += 1;
                _unitRpository.Update(model.Unit);
                model.DateFrom = DateFrom;
                model.DateTo = DateTo;
                if (model.DateTo.Date == model.DateFrom.Date || model.DateTo.Date < model.DateFrom.Date)
                {
                    model.DateTo = DateFrom.AddDays(1);
                }
                model.Rates = _rateRepository.Table.Include(c => c.User).Where(c => c.UnitId == id).ToList();
                model.Rate.Cleaning = model.Rates.Sum(c => c.Cleaning) / model.Rates.Count();
                model.Rate.Crew = model.Rates.Sum(c => c.Crew) / model.Rates.Count();
                model.Rate.PropertyCondition = model.Rates.Sum(c => c.PropertyCondition) / model.Rates.Count();
                if (SessionClass.IsAuthentecated(HttpContext))
                {
                    var user = SessionClass.GetUser(HttpContext);
                    ViewBag.hasReservation = _reservationRepository.Table.Any(c => c.UserId == user.Id && c.UnitId == model.Unit.Id && c.Status == (int)Enums.Status.Confirmed);
                }
                else
                {
                    ViewBag.hasReservation = false;
                }

                foreach (var item in model.Unit.ChaletParameterValues)
                {
                    if (!model.ParameterGroups.Contains(item.Parameter.ParameterGroup))
                    {
                        model.ParameterGroups.Add(item.Parameter.ParameterGroup);
                    }
                }
                foreach (var item in model.ParameterGroups)
                {
                    item.Parameters = new List<Parameter>();
                    var parameters = model.Unit.ChaletParameterValues.Where(c => c.Parameter.ParameterGroupId == item.Id);
                    foreach (var param in parameters)
                    {
                        if (!string.IsNullOrEmpty(param.Value))
                        {
                            param.Parameter.Value = param.Value;
                            param.Parameter.Order = param.Order;
                            if (!item.Parameters.Any(c => c.Id == param.ParameterId))
                            {
                                item.Parameters.Add(param.Parameter);
                            }
                        }
                    }

                }

                //if prority type is resort
                if (model.Unit.Chalet.PropertyType==(int)Enums.PropertyType.Resort)
                {
                    foreach (var item in model.Unit.Chalet.ResortParameterValue)
                    {
                        if (!model.ResortParameterGroups.Contains(item.Parameter.ParameterGroup))
                        {
                            model.ResortParameterGroups.Add(item.Parameter.ParameterGroup);
                        }
                    }

                    foreach (var item in model.ResortParameterGroups)
                    {
                        item.Parameters = new List<Parameter>();
                        var parameters = model.Unit.Chalet.ResortParameterValue.Where(c => c.Parameter.ParameterGroupId == item.Id);
                        foreach (var param in parameters)
                        {
                            if (!string.IsNullOrEmpty(param.Value))
                            {
                                param.Parameter.Value = param.Value;
                                if (!item.Parameters.Any(c => c.Id == param.ParameterId))
                                {
                                    item.Parameters.Add(param.Parameter);
                                }
                            }
                        }

                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Unit/Details/5
        public ActionResult Reserve(Guid id, DateTime CheckIn, DateTime CheckOut)
        {
            SingleUnitViewModel model = new SingleUnitViewModel();
            try
            {
                if (SessionClass.IsAuthentecated(HttpContext))
                {
                    var user = SessionClass.GetUser(HttpContext);
                    if (user.UserType != (int)Enums.UserType.EndUser)
                    {
                        Error(Resource.YouCannotLoginWithBookingAccount);
                        return RedirectToAction("Index", new { id = id, DateFrom = CheckIn, DateTo = CheckOut });
                    }
                }
                ViewBag.CodeSent = false;
                model.Unit = _unitRpository.Table
                    .Include(c => c.Chalet.City)
                    .Include("UnitImages.File")
                    .FirstOrDefault(c => c.Id == id);
                model.Unit.IsReserved = _reservationRepository.Any(c => c.UnitId == model.Unit.Id
                 && (c.DateFrom.Date <= CheckIn.Date && c.DateTo.Date >= CheckIn.Date)
                 && (c.DateFrom.Date <= CheckOut.Date && c.DateTo.Date >= CheckOut.Date));
                if (_reservationRepository.Any(c => c.UnitId == model.Unit.Id && c.DateFrom.Date >= CheckIn.Date && c.DateTo.Date <= CheckOut.Date))
                {
                    Error(Resource.UnitReserved);
                    return RedirectToAction("Index","Home");
                }
                model.DateFrom = CheckIn;
                model.DateTo = CheckOut;
                if (model.DateTo.Date == model.DateFrom.Date || model.DateTo.Date < model.DateFrom.Date)
                {
                    model.DateTo = model.DateFrom.AddDays(1);
                }
                model.Rates = _rateRepository.Table.Include(c => c.User).Where(c => c.UnitId == id).ToList();
                model.Rate.Cleaning = model.Rates.Sum(c => c.Cleaning) / model.Rates.Count();
                model.Rate.Crew = model.Rates.Sum(c => c.Crew) / model.Rates.Count();
                model.Rate.PropertyCondition = model.Rates.Sum(c => c.PropertyCondition) / model.Rates.Count();

                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // GET: Unit/Details/5
        [Authrize("EndUser")]
        public ActionResult Payment(Guid id, DateTime CheckIn, DateTime CheckOut)
        {
            SingleUnitViewModel model = new SingleUnitViewModel();
            try
            {
                string referer = Request.Headers["Referer"].ToString();
                if (CheckRefere(referer))
                {
                    return RedirectToAction("Index","Home");
                }
                var user = SessionClass.GetUser(HttpContext);
                if (user.UserType != (int)Enums.UserType.EndUser)
                {
                    Error(Resource.YouCannotLoginWithBookingAccount);
                    return RedirectToAction("Index", new { id = id, DateFrom = CheckIn, DateTo = CheckOut });
                }
                ViewBag.CodeSent = false;
                model.Unit = _unitRpository.Table
                    .Include(c => c.Chalet.City)
                    .Include("UnitImages.File")
                    .FirstOrDefault(c => c.Id == id);
                model.Unit.IsReserved = _reservationRepository.Any(c => c.UnitId == model.Unit.Id
                 && (c.DateFrom.Date <= CheckIn.Date && c.DateTo.Date >= CheckIn.Date)
                 && (c.DateFrom.Date <= CheckOut.Date && c.DateTo.Date >= CheckOut.Date));
                if (model.Unit.IsReserved)
                {
                    Error(Resource.UnitReserved);
                    return RedirectToAction("Reservations", "User");
                }
                model.DateFrom = CheckIn;
                model.DateTo = CheckOut;
                if (model.DateTo.Date == model.DateFrom.Date || model.DateTo.Date < model.DateFrom.Date)
                {
                    model.DateTo = model.DateFrom.AddDays(1);
                }
                model.Rates = _rateRepository.Table.Include(c => c.User).Where(c => c.UnitId == id).ToList();
                model.Rate.Cleaning = model.Rates.Sum(c => c.Cleaning) / model.Rates.Count();
                model.Rate.Crew = model.Rates.Sum(c => c.Crew) / model.Rates.Count();
                model.Rate.PropertyCondition = model.Rates.Sum(c => c.PropertyCondition) / model.Rates.Count();
                return View(model);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private bool CheckRefere(string referer)
        {
            if (string.IsNullOrEmpty(referer))
            {
                return false;
            }
            var url = referer.Split("/");
            var controller = url[3];
            var action = url[4];
            if (controller!="unit" || action!= "reserve")
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [Authrize("EndUser")]
        public ActionResult Payment(int type,string ccv,string exDate,string cardNo, Guid unitId, DateTime CheckIn, DateTime CheckOut)
        {
            try
            {
                var user = SessionClass.GetUser(HttpContext);
                var unit = _unitRpository.Table
                    .Include(c => c.Chalet)
                    .FirstOrDefault(c => c.Id == unitId);
                var reservation = new Reservation();
                reservation.ReservedBy = (int)Enums.ReservedBy.Website;
                reservation.DateFrom = CheckIn;
                reservation.DateTo = CheckOut;
                reservation.UnitId = unit.Id;
                if (unit.IsDayPrice)
                {
                    reservation.DayPrice = unit.DayPrice;
                    reservation.TotalPrice = reservation.DayPrice * reservation.TotalDays;
                    //reservation.DayPrice = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == unitId);
                }
                else
                {
                    reservation.DayPrice = unit.DayPrice;
                    reservation.TotalPrice = reservation.DayPrice * reservation.TotalDays;
                }
                var dates = CheckOut.Date - CheckIn.Date;
                reservation.TotalDays = Math.Abs(dates.Days);
                reservation.ReservedByUser = user.FirstName + " " + user.LastName;
                reservation.UserId = user.Id;
                reservation.Status = (int)Enums.Status.New;
                _reservationRepository.Add(reservation);
                Success(Resource.ReservedSuccessfully);
                return RedirectToAction("Reservations","User");
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost]

        public IActionResult Reserve(SingleUnitViewModel model, string mobileno, int code, DateTime DateFrom, DateTime DateTo, bool confirm = false)
        {
            try
            {
                model.Unit = _unitRpository.Table
                    .Include(c => c.Chalet.City)
                    .Include("UnitImages.File")
                    .FirstOrDefault(c => c.Id == model.Id);

                var user = _userRepository.Table.FirstOrDefault(c => c.PhoneNumber == mobileno);
                if (user != null)
                {
                    if (user.UserType != (int)Enums.UserType.EndUser)
                    {
                        ViewBag.MobileNo = mobileno;
                        ViewBag.IsExist = false;
                        ViewBag.CodeSent = false;

                        Error(Resource.YouCannotLoginWithBookingAccount);
                        return View(model);
                    }
                }
                if (confirm)
                {
                    if (user.ConfirmCode == code)
                    {
                        SessionClass.SetUser(HttpContext, user);
                        return View(model);
                    }
                    if (user.IsConfirmed)
                    {
                        ViewBag.IsExist = true;
                    }
                    else
                    {
                        ViewBag.IsExist = false;
                    }
                    ViewBag.MobileNo = mobileno;
                    ViewBag.CodeSent = true;
                    ViewBag.Code = user.ConfirmCode;
                    Error(Resource.WrongeVerificationCode);
                    return View(model);
                }
                if (user == null)
                {
                    user = new User();
                    user.UserType = (int)Enums.UserType.EndUser;
                    user.Email = "";
                    user.Status = true;
                    user.IsAdmin = false;
                    user.BirthDate = DateTime.Now.Date;
                    user.TemporaryPassword = false;
                    user.CreatedDate = DateTime.Now;
                    user.IsConfirmed = false;
                    user.PhoneNumber = mobileno;
                    user.ConfirmCode = long.Parse(getCode());
                    _userRepository.Add(user);
                    ViewBag.IsExist = false;
                }
                else
                {
                    user.ConfirmCode = long.Parse(getCode());
                    _userRepository.UserUpdate(user);
                    ViewBag.IsExist = true;
                }
                ViewBag.MobileNo = mobileno;
                ViewBag.CodeSent = true;
                ViewBag.Code = user.ConfirmCode;
                return View(model);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult Units(int? type, Guid? city, DateTime? checkIn, DateTime? checkOut)
        {
            try
            {
                if (city == null)
                {
                    city = Guid.Empty;

                }
                else
                {
                    ViewBag.CityName = _cityRepository.Find(city)?.CityName;
                }
                if (checkIn == null)
                {
                    checkIn = DateTime.Now;
                }
                if (checkOut == null)
                {
                    checkOut = DateTime.Now.AddDays(1);
                }
                var dates = (DateTime)checkIn - (DateTime)checkOut;
                ViewBag.Nights = Math.Abs(dates.Days);
                ViewBag.Type = type;
                ViewBag.City = city;
                ViewBag.CheckIn = checkIn;
                ViewBag.CheckOut = checkOut;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public IActionResult GetProperty(int? type, Guid city, DateTime? checkIn, DateTime? checkOut, int pageNumber = 1)
        {
            try
            {
                SearchItemViewModel searchItemViewModel = new SearchItemViewModel();
                searchItemViewModel.PageSize = 12;
                searchItemViewModel.PageNumber = pageNumber;
                var excludeRecord = (searchItemViewModel.PageSize * pageNumber) - searchItemViewModel.PageSize;
                if (type!=null&&type==(int)Enums.PropertyType.MainResort)
                {
                    var offers = _offersRepository.Table.Where(c=>
                    c.Unit.Chalet.IsConfirmed && c.Unit.Chalet.ViewStatus&&c.Unit.ViewStatus
                    &&(checkIn==null|| c.DateFrom.Date <= checkIn&&c.DateTo.Date>=checkIn)
                    &&(checkOut == null || c.DateFrom.Date <= checkOut && c.DateTo.Date >= checkOut)).Include(c=>c.Unit.Offers).Include("Unit.UnitImages.File").Include(c=>c.Unit.Chalet.City).ToList();
                    searchItemViewModel.Units = offers.Select(c => c.Unit).Skip(excludeRecord).Take(searchItemViewModel.PageSize).ToList();
                    searchItemViewModel.TotalRecord = offers.Select(c => c.Unit).Count();
                }
                else { 
                //var dates = Date.Split(" - ");
                searchItemViewModel.Units = _unitRepository.Table.Include(c=>c.PricePerDays).Include(c=>c.Offers).Include("UnitImages.File").Include(c => c.Chalet.City)
                    .Where(c =>c.Chalet.IsConfirmed&&c.Chalet.ViewStatus && c.ViewStatus
                    && (type == null || c.Chalet.PropertyType == type)
                    && (city == Guid.Empty || c.Chalet.CityId == city)).Skip(excludeRecord).Take(searchItemViewModel.PageSize).ToList();
                    if (checkIn==null)
                    {
                        checkIn = DateTime.Now.Date;
                        checkOut = DateTime.Now.Date;
                    }
                    foreach (var myUnit in searchItemViewModel.Units)
                    {
                        myUnit.IsReserved = _reservationRepository.Any(c => c.UnitId == myUnit.Id 
                        && (checkIn == null || c.DateFrom.Date <= checkIn && c.DateTo.Date >= checkIn) 
                        && (checkOut == null || c.DateFrom.Date <= checkOut && c.DateTo.Date >= checkOut));
                        DateConverter.GetPricePerDay(HttpContext,myUnit,(int)DateTime.Now.DayOfWeek);
                    }

                    searchItemViewModel.TotalRecord = _unitRepository.Table
                    .Count(c => (type == null || c.Chalet.PropertyType == type)
                    && (city == Guid.Empty || c.Chalet.CityId == city));
                }
                return PartialView("_proprties", searchItemViewModel);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(SingleUnitViewModel model)
        {
            try
            {
                string code = getCode();
                if (!string.IsNullOrEmpty(model.Email))
                {
                    var user = _userRepository.Table.FirstOrDefault(c => c.Email == model.Email);
                    if (user != null)
                    {
                        Error(Resource.UserAlreadyExists);
                        return RedirectToAction(nameof(Reserve), new { id = model.Unit.Id, CheckIn = model.DateFrom, CheckOut = model.DateTo });
                    }
                    user = new User();
                    user.PhoneNumber = model.PhoneNumber;
                    user.Password = model.Password;
                    user.UserType = model.UserType;
                    user.Email = model.Email;
                    user.ConfirmCode = long.Parse(code);
                    await _userRepository.AddAsync(user);
                    return RedirectToAction(nameof(Reserve), new { id = model.Unit.Id, CheckIn = model.DateFrom, CheckOut = model.DateTo });
                }
                else
                {
                    Error(Resource.Validations);
                }
                return RedirectToAction(nameof(Reserve), new { id = model.Unit.Id, CheckIn = model.DateFrom, CheckOut = model.DateTo });

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public JsonResult Fiverate(Guid id)
        {
            try
            {
                return Json(Domain.Fiverates.AddRemoveFive(HttpContext, id));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [Authrize("EndUser")]
        public IActionResult ReservationDetails(Guid id)
        {
            try
            {
                var reservation = _reservationRepository.Table.Include(c => c.Unit).ThenInclude(c => c.Chalet).Include(c => c.User).Include(c=>c.Invoices).FirstOrDefault(c => c.Id == id);
                return View(reservation);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string getCode()
        {
            Random generator = new Random();
            return generator.Next(1, 1000000).ToString("D6");
        }
    }
}
