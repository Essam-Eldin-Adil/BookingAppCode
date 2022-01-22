using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models.Chalets;
using Data.Models.Chalets.ChaletDetails;
using Data.ViewModels;
using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;
using File = Data.Models.File;
using static BookingApp.Authrize;

namespace BookingApp.Controllers
{
    [Authrize("Admin,BookAdmin,BookUser")]
    public class ChaletsController : BaseController
    {
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<Chalet> _chaletRepository;
        private readonly IRepository<ChaletImage> _chaletImageRepository;
        private readonly IRepository<Offer> _offerRepository;
        private readonly IRepository<PricePerDay> _pricePerDayRepository;
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<UnitImage> _unitImageRepository;
        private readonly IRepository<ParameterGroup> _groupRepository;
        private readonly IRepository<ChaletParameterValue> _chaletParameterValueRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<User> _userRepository;
        public ChaletsController(IRepository<Unit> unitRepository, IRepository<Chalet> chaletRepository, IRepository<ChaletImage> chaletImageRepository, IRepository<Offer> offerRepository, IRepository<PricePerDay> pricePerDayRepository, IRepository<UnitImage> unitImageRepository, IRepository<File> fileRepository, IRepository<ParameterGroup> groupRepository, IRepository<ChaletParameterValue> chaletParameterValueRepository, IRepository<Parameter> parameterRepository, IRepository<Reservation> reservationRepository, IRepository<User> userRepository)
        {
            _unitRepository = unitRepository;
            _chaletRepository = chaletRepository;
            _chaletImageRepository = chaletImageRepository;
            _offerRepository = offerRepository;
            _pricePerDayRepository = pricePerDayRepository;
            _unitImageRepository = unitImageRepository;
            _fileRepository = fileRepository;
            _groupRepository = groupRepository;
            _chaletParameterValueRepository = chaletParameterValueRepository;
            _parameterRepository = parameterRepository;
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
        }


        public IActionResult Index(Guid id)
        {
            try
            {
                UnitsViewModel model=new UnitsViewModel();
                model.Chalet = _chaletRepository.Find(id);
                //if (!model.Chalet.IsConfirmed)
                //{
                //    Warning(Resource.ProrpertNotConfirmed);
                //}
                model.Units = _unitRepository.Table.Where(c => c.ChaletId == id).ToList();
                model.ChaletImages = _chaletImageRepository.Table.Include(c=>c.File).Where(c => c.ChaletId == id).ToList();
                if (model.Chalet.PropertyType == (int)Enums.PropertyType.Resort)
                {
                    ViewBag.IsResort = model.Chalet.Id;
                }
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult ProprityProfile(Guid id)
        {
            try
            {
                var propirty = _chaletRepository.Table.FirstOrDefault(c => c.Id == id);
                return View(propirty);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Unit(Guid? id,Guid chaletId)
        {
            UnitViewModel model = new UnitViewModel();
            try
            {
                var chalet = _chaletRepository.Find(chaletId);
                if (chalet == null)
                {
                    return NotFound();
                }
                ViewBag.PropertyType = chalet.PropertyType;

                model.ParameterGroups = _groupRepository.Table.Include(c => c.Parameters).ToList();
                if (id==null)
                {
                    model.ChaletId = chaletId;
                    return View(model);
                }

                
                model.Unit = _unitRepository.Find(id);
                if (model.Unit == null)
                {
                    model.ChaletId = chaletId;
                    return View(model);
                }
                model.ChaletId = chaletId;
                model.Offers = _offerRepository.Table.Where(c => c.UnitId == id).ToList();
                model.PricePerDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == id);
                if (model.PricePerDay==null)
                {
                    model.PricePerDay = new PricePerDay();
                }
                model.SimilarUnits = _unitRepository.Table.Include("UnitImages.File").Where(c => c.OriginId == model.Unit.Id).ToList();
                model.ChaletParameterValues = _chaletParameterValueRepository.Table.Where(c=>c.UnitId==model.Unit.Id).ToList();
                model.UnitImage = _unitImageRepository.Table.Include(c=>c.File).Where(c => c.UnitId == id).ToList();
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveUnit(UnitViewModel model, string[] inputValues,Guid[] inputIds,Guid[] checkboxes)
        {
            try
            {
                var isHasImages = false;
                var unit = _unitRepository.Find(model.Unit.Id);
                if (unit == null)
                {
                    model.Unit.Number = 1;
                    model.Unit.ChaletId = model.ChaletId;
                    _unitRepository.Add(model.Unit);
                    unit = model.Unit;
                    unit.ChaletId = model.ChaletId;
                }
                else
                {
                    unit.MaximumAllowed = model.Unit.MaximumAllowed;
                    unit.MoreThanAllowed = model.Unit.MoreThanAllowed;
                    unit.MoreThanAllowedPrice = model.Unit.MoreThanAllowedPrice;
                    unit.AllowedPersons = model.Unit.AllowedPersons;
                    unit.ViewStatus = model.Unit.ViewStatus;
                    unit.Code = model.Unit.Code;
                    unit.Name = model.Unit.Name;
                    unit.Count = model.Unit.Count;
                    unit.HaveSimilar = model.Unit.HaveSimilar;
                    unit.DepositAmount = model.Unit.DepositAmount;
                    unit.Description = model.Unit.Description;
                    unit.DayPrice = model.Unit.DayPrice;
                    unit.Space = model.Unit.Space;
                    unit.Description = model.Unit.Description;
                    _unitRepository.Update(unit);
                }

                var perDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == model.Unit.Id);
                if (perDay==null)
                {
                    perDay=new PricePerDay();
                    _pricePerDayRepository.Add(model.PricePerDay);
                }
                else
                {
                    perDay.Friday = model.PricePerDay.Friday;
                    perDay.Monday = model.PricePerDay.Monday;
                    perDay.Saturday = model.PricePerDay.Saturday;
                    perDay.Sunday = model.PricePerDay.Sunday;
                    perDay.Thursday = model.PricePerDay.Thursday;
                    perDay.Wednesday = model.PricePerDay.Wednesday;
                    perDay.Tuesday = model.PricePerDay.Tuesday;
                    _pricePerDayRepository.Update(perDay);
                }
                isHasImages = _unitImageRepository.Table.Any(c => c.UnitId == model.Unit.Id);
                var files = Request.Form.Files;
                var filesId = Domain.File.Upload("UnitImages", _fileRepository, files);
                foreach (var id in filesId)
                {
                    var unitImage = new UnitImage();
                    unitImage.UnitId = model.Unit.Id;
                    unitImage.FileId = id;
                    if (!isHasImages)
                    {
                        isHasImages = true;
                        if (filesId.IndexOf(id) == 0)
                        {
                            unitImage.IsPrimary = true;
                        }
                    }
                    _unitImageRepository.Add(unitImage);
                }

                BindParameter(inputValues,inputIds,checkboxes, unit.Id);

                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Unit", new { id = model.Unit.Id,chaletId=model.ChaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveSimilarChalet(Guid id, Guid originalChaletId,string chaletName,string chaletCode,bool chaletViewStatus)
        {
            try
            {
                var similarUnit = _unitRepository.Find(id);
                var originalUnit = _unitRepository.Find(originalChaletId);
                var number = 2;
                var  unit = _unitRepository.Table.Where(c => c.OriginId == originalChaletId).ToList();
                if (unit.Count>0)
                {
                    number = unit.Max(c => c.Number) + 1;
                }
                if (originalUnit==null)
                {
                    Error(Resource.AlertErrorSavingData);
                    return RedirectToAction("Index","Home");
                }
                if (similarUnit==null)
                {
                    similarUnit = new Unit
                    {
                        Number = number,
                        Id = Guid.NewGuid(), ViewStatus = chaletViewStatus, Name = chaletName, Code = chaletCode,OriginId = originalUnit.Id,IsSimilar = true,ChaletId = originalUnit.ChaletId
                    };
                    _unitRepository.Add(similarUnit);
                }
                else
                {
                    similarUnit.ViewStatus = chaletViewStatus;
                    similarUnit.Name = chaletName;
                    similarUnit.Code = chaletCode;
                    _unitRepository.Update(similarUnit);
                }
                var files = Request.Form.Files;
                if (files.Count>0)
                {
                    var images = _unitImageRepository.Table.Where(c => c.UnitId == similarUnit.Id);
                    foreach (var unitImage in images)
                    {
                        Domain.File.Remove(_fileRepository, unitImage.FileId);
                    }
                    var filesId = Domain.File.Upload("UnitImages", _fileRepository, files);
                    foreach (var fileId in filesId)
                    {
                        var unitImage = new UnitImage {UnitId = similarUnit.Id, FileId = fileId, IsPrimary = true};
                        _unitImageRepository.Add(unitImage);
                    }
                }




                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Unit", new {id= originalChaletId, chaletId= originalUnit.ChaletId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult RemoveUnit(Guid id, string referUrl)
        {
            try
            {
                var similarUnit = _unitRepository.Find(id);
                if (similarUnit != null)
                {
                    _unitRepository.RemoveSoft(similarUnit);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(referUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [HttpPost]
        public IActionResult AddOffer(UnitViewModel model,Guid chaletId)
        {
            try
            {
                _offerRepository.Add(model.Offer);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Unit", new { id = model.Offer.UnitId, chaletId = chaletId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult RemoveOffer(Guid id,Guid unitId, Guid chaletId)
        {
            try
            {
                var offer = _offerRepository.Find(id);
                _offerRepository.Remove(offer);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Unit", new { id = unitId, chaletId = chaletId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void BindParameter(IReadOnlyList<string> inputValues, IReadOnlyList<Guid> inputIds, IEnumerable<Guid> checkboxes, Guid unitId)
        {
            var unitParameters = _chaletParameterValueRepository.Table.Where(c => c.UnitId == unitId).ToList();
            _chaletParameterValueRepository.RemoveHardRange(unitParameters);

            for (var i = 0; i < inputIds.Count; i++)
            {
                var paramValues = new ChaletParameterValue
                {
                    UnitId = unitId, ParameterId = inputIds[i], Value = inputValues[i]
                };
                _chaletParameterValueRepository.Add(paramValues);
            }

            foreach (var checkbox in checkboxes)
            {
                var paramValues = new ChaletParameterValue
                {
                    UnitId = unitId,
                    ParameterId = checkbox,
                    Value = "1"
                };
                _chaletParameterValueRepository.Add(paramValues);
            }

        }

        [HttpGet]
        public IActionResult RemoveUnitImage(Guid id, Guid unitId, Guid chaletId)
        {
            Domain.File.Remove(_fileRepository, id);
            var unitImage = _unitImageRepository.Table.FirstOrDefault(c=>c.FileId==id);
            if (unitImage != null)
            {
                _unitImageRepository.Remove(unitImage);
            }
            Success(Resource.AlertDataSavedSuccessfully);
            return RedirectToAction("Unit", new { id = unitId,chaletId=chaletId });
        }

        [HttpGet]
        public IActionResult Reservations(Guid id)
        {
            try
            {
                var units = _unitRepository.Table.Where(c => c.ChaletId == id).Select(c=>c.Id).ToList();
                var reservations = _reservationRepository.Table.Include(c=>c.User).Include("Unit.Chalet.ChaletImages.File").Where(c => units.Contains(c.UnitId))
                    .ToList();
                ViewBag.chaletId = id;
                var chalet = _chaletRepository.Find(id);
                //if (!chalet.IsConfirmed)
                //{
                //    Warning(Resource.ProrpertNotConfirmed);
                //}
                if (chalet.PropertyType == (int)Enums.PropertyType.Resort)
                {
                    ViewBag.IsResort = chalet.Id;
                }
                return View(reservations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult Calender(Guid id)
        {
            try
            {
                ViewBag.units = new SelectList(_unitRepository.Table.Where(c=>c.ChaletId==id&&!c.IsDeleted),"Id","Name");
                ViewBag.chaletId = id;
                var chalet = _chaletRepository.Find(id);
                //if (!chalet.IsConfirmed)
                //{
                //    Warning(Resource.ProrpertNotConfirmed);
                //}
                if (chalet.PropertyType == (int)Enums.PropertyType.Resort)
                {
                    ViewBag.IsResort = chalet.Id;
                }
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult GetCalenderData(Guid chaletId, int year, int month)
        {
            try
            {
                
                var days = DateTime.DaysInMonth(year, month);
                var unitsCount = _unitRepository.Table.Count(c => c.ChaletId == chaletId);
                var data = new List<CalenderJson>();
                for (var i = 1; i < days+1; i++)
                {
                    var date = new DateTime(year, month, i);
                    var reservation = _reservationRepository.Table.Where(c =>
                        c.Unit.ChaletId == chaletId && (c.DateFrom.Date <= date.Date) &&
                        (c.DateTo.Date >= date.Date)).ToList(); 
                    if (date.Date<DateTime.Now.Date)
                    {
                        data.Add(new CalenderJson
                        {
                            Date = date.ToString("MM-dd-yyy"),
                            Description = $"<a class='btn btn-secondary btn-sm disabled'>{Resource.AddReservation} {reservation.Count()}/{unitsCount}</a>"
                        });
                        continue;
                    }
                    
                    data.Add(reservation.Count() >= unitsCount
                        ? new CalenderJson
                        {
                            Date = date.ToString("MM-dd-yyy"),
                            Description = $"<a href='{Url.Action("Reservation", new { id = chaletId, date })}' class='btn btn-danger btn-sm'>{Resource.AddReservation} {reservation.Count()}/{unitsCount}</a>"
                        }
                        : new CalenderJson
                        {
                            Date = date.ToString("MM-dd-yyy"),
                            Description = $"<a href='{Url.Action("Reservation",new {id=chaletId, date })}' class='btn btn-success btn-sm'>{Resource.AddReservation} {reservation.Count()}/{unitsCount}</a>"
                        });
                }
                //<button>Reserve</button>
                return Json(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult Reservation(Guid id, DateTime date)
        {
            try
            {
                var model = new ReservationViewModel();
                var units = _unitRepository.Table.Include(c=>c.Chalet).Include("UnitImages.File").Where(c => c.ChaletId == id).ToList();
                model.Chalet = _chaletRepository.Find(id);
                model.Reservations = _reservationRepository.Table.Include(c=>c.User).Where(c =>
                    c.Unit.ChaletId == id && (c.DateFrom.Date <= date.Date) &&
                    (c.DateTo.Date >= date.Date)).ToList() ;
                model.Date = date;
                foreach (var unit in units)
                {
                    var reservation = model.Reservations.Any(c =>
                        c.UnitId == unit.Id && (c.DateFrom.Date <= date.Date) &&
                        (c.DateTo.Date >= date.Date));
                    if (!reservation && model.SelectedUnit == null)
                    {
                        model.SelectedUnit = unit;
                    }
                    model.ReservationModels.Add(new ReservationModel
                    {
                        Unit = unit,
                        Available = !reservation
                    });
                }
                if (model.Chalet.PropertyType == (int)Enums.PropertyType.Resort)
                {
                    ViewBag.IsResort = model.Chalet.Id;
                }
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        [HttpPost]
        public IActionResult Payment(Guid ReservationId, double Amount)
        {
            try
            {
                var reservation = _reservationRepository.Table.Include(c=>c.Unit).FirstOrDefault(c=>c.Id==ReservationId);
                reservation.Status = (int)Enums.Status.Confirmed;
                _reservationRepository.ReservationUpdate(reservation);
                Domain.Payment.PayCash(HttpContext, Amount, ReservationId,SessionClass.GetUser(HttpContext).Id);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Reservations",new { id= reservation.Unit.ChaletId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private class CalenderJson
        {
            public string Date { get; set; }
            public string Description { get; set; }
        }

        [HttpPost]
        public IActionResult AddReservation(Guid unitId,Guid chaletId, string firstName,string lastName,string phoneNumber,double reservationPrice,string description,DateTime CheckIn,DateTime CheckOut)
        {
            try
            {
                var user = _userRepository.Table.FirstOrDefault(c => c.PhoneNumber == phoneNumber&&c.FirstName==firstName);
                if (user==null)
                {
                    user = new User
                    {
                        PhoneNumber = phoneNumber,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = "",
                        IsAdmin = false,
                        UserType = (int)Enums.UserType.EndUser
                    };
                    _userRepository.Add(user);
                }
                var entryUser = SessionClass.GetUser(HttpContext);
                var isReserved = _reservationRepository.Table.Any(c=>(c.DateFrom.Date <= CheckIn && c.DateTo.Date >= CheckIn) &&(c.DateFrom.Date <= CheckOut && c.DateTo.Date >= CheckOut));
                if (isReserved)
                {
                    Error(Resource.UnitReserved);
                    return RedirectToAction(nameof(Reservation), new { id = chaletId, date = CheckIn });
                }
                var reservation = new Reservation
                {
                    UnitId = unitId,
                    DateFrom = CheckIn,
                    DateTo = CheckIn== CheckOut? CheckOut.AddDays(1) : CheckOut,
                    UserId = user.Id,
                    Status = (int) Enums.Status.Confirmed,
                    ReservedBy = (int) Enums.ReservedBy.ProprityUser,
                    ReservedByUser = entryUser.FirstName+" "+ entryUser.LastName
                };
                _reservationRepository.Add(reservation);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Reservation),new {id=chaletId, date = CheckIn });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult CancelReservation(Guid id)
        {
            try
            {
                var reservation = _reservationRepository.Table.Include(c=>c.Unit).FirstOrDefault(c=>c.Id==id);
                return View(reservation);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult CancelReservation(Reservation model,string resones)
        {
            try
            {
                var reservation = _reservationRepository.Table.Include(c => c.Unit).FirstOrDefault(c => c.Id == model.Id);
                reservation.Status = (int)Enums.Status.Cancled;
                reservation.CancelResones = resones;
                return View(reservation);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ReservationDetails(Guid id)
        {
            try
            {
                var reservation = _reservationRepository.Table.Include(c => c.Unit).ThenInclude(c => c.Chalet).Include(c => c.User).Include(c => c.Invoices).ThenInclude(c=>c.User).FirstOrDefault(c => c.Id == id);
                return View(reservation);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
