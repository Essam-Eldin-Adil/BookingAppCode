using Data.Models;
using Data.Models.Chalets;
using Data.Models.Chalets.RatingAndReview;
using Data.Models.General;
using Data.ViewModels;
using Domain;
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
    public class UserController : BaseController
    {
        private readonly IRepository<Data.Models.File> _fileRepository;
        private readonly IRepository<ContactUs> _contactUsRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Fiverate> _fiveRepository;
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<Rate> _rateRepository;


        public UserController(IRepository<Reservation> reservationRepository,IRepository<Fiverate> fiveRepository,IRepository<User> userRepository,
            IRepository<ContactUs> contactUsRepository,
            IRepository<Rate> rateRepository,
            IRepository<Data.Models.File> fileRepository)
        {
            _rateRepository = rateRepository;
            _fileRepository = fileRepository;
            _userRepository = userRepository;
            _contactUsRepository = contactUsRepository;
            _fiveRepository = fiveRepository;
            _reservationRepository = reservationRepository;
        }
        public IActionResult Join(string returnUrl = "")
        {
            ViewBag.CodeSent = false;
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        public IActionResult Join(string mobileno,int code, string returnUrl = "", bool confirm=false)
        {
            var user = _userRepository.Table.FirstOrDefault(c => c.PhoneNumber == mobileno);
            ViewBag.returnUrl = returnUrl;
            //if (user != null)
            //{
            //    if (user.UserType != (int)Enums.UserType.EndUser)
            //    {
            //        Error(Resource.YouCannotLoginWithBookingAccount);
            //        return View();
            //    }
            //}
            if (confirm)
            {
                if (user.ConfirmCode == code)
                {
                    SessionClass.SetUser(HttpContext, user);
                    if (user.IsConfirmed)
                    {
                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction(nameof(CompleteRegister), new { id = user.Id });
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
                return View();
            }
            if (user==null)
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
            return View();
        }

        [Authrize("EndUser")]
        public IActionResult Profile()
        {
            var model = new UserAccountViewModel();
            var user = SessionClass.GetUser(HttpContext);
            model.User = _userRepository.Find(user.Id);
            return View(model);
        }

        [Authrize("EndUser")]
        public IActionResult Infos()
        {
            var user = SessionClass.GetUser(HttpContext);
            ViewBag.Reservations = _reservationRepository.Table.Count(c => c.UserId == user.Id);
            ViewBag.Fiverates = _fiveRepository.Table.Count(c => c.UserId == user.Id);
            var model = _reservationRepository.Table.Include(c => c.Unit.Chalet.City).Include("Unit.UnitImages.File").Where(c => c.UserId == user.Id&&c.Status==(int)Enums.Status.New);
            return View(model);
        }

        [Authrize("EndUser")]
        public IActionResult Reservations()
        {
            List<UserReservationViewModel> model = new List<UserReservationViewModel>();
            var user = SessionClass.GetUser(HttpContext);
            var reservations = _reservationRepository.Table.Include(c => c.Unit.Chalet.City).Include("Unit.UnitImages.File").Where(c => c.UserId == user.Id).ToList();
            foreach (var item in reservations)
            {
                var rate = _rateRepository.Table.FirstOrDefault(c => c.UserId == user.Id && c.UnitId == item.UnitId);
                var rates = _rateRepository.Table.Where(c => c.UnitId == item.UnitId).ToList();
                var itemModel = new UserReservationViewModel
                {
                    Unit = item.Unit,
                    Id = item.Id,
                    UnitId = item.UnitId,
                    CancelResones = item.CancelResones,
                    CreatedDate = item.CreatedDate,
                    DateFrom = item.DateFrom,
                    DateTo = item.DateTo,
                    DayPrice = item.DayPrice,
                    IsDeleted = item.IsDeleted,
                    Order = item.Order,
                    ReservationNumber = item.ReservationNumber,
                    ReservedBy = item.ReservedBy,
                    ReservedByUser = item.ReservedByUser,
                    Status = item.Status,
                    TotalDays = item.TotalDays,
                    TotalPrice = item.TotalPrice,
                    UserId = item.UserId,
                    UserRate = rate==null?new Data.Models.Chalets.RatingAndReview.Rate():rate,
                    RateCount = rates.Count,
                    CleaningRate= rates.Sum(c=>c.Cleaning)/ rates.Count,
                    CrewRate = rates.Sum(c=>c.Crew) / rates.Count,
                    PropertyConditionRate = rates.Sum(c=>c.PropertyCondition) / rates.Count,
                };
                model.Add(itemModel);
            }
            return View(model);
        }


         [Authrize("EndUser")]
         [HttpPost]
        public IActionResult CancelReservation(Guid ReservationId,string CancelResoun)
        {

            try
            {
                var reservation = _reservationRepository.Find(ReservationId);
                if (reservation != null)
                {
                    reservation.Status = (int)Enums.Status.Cancled;
                    reservation.CancelResones = CancelResoun;
                    _reservationRepository.ReservationUpdate(reservation);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Reservations");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]

        public IActionResult Rate(Rate model)
        {
            try
            {
                var rate = _rateRepository.Find(model.Id);
                if (rate==null)
                {
                    _rateRepository.Add(model);
                }
                else
                {
                    rate.PropertyCondition = model.PropertyCondition;
                    rate.Cleaning = model.Cleaning;
                    rate.Crew = model.Crew;
                    rate.Comment = model.Comment;
                    _rateRepository.Update(rate);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Reservations");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authrize("EndUser")]
        public IActionResult Fiverates()
        {
            var user = SessionClass.GetUser(HttpContext);
            var model = _fiveRepository.Table.Include(c=>c.Unit.Chalet.City).Include("Unit.UnitImages.File").Where(c=>c.UserId==user.Id);
            return View(model);
        }
        [Authrize("EndUser")]
        [HttpPost]
        public IActionResult SaveProfile(User model)
        {
            try
            {
                var user = _userRepository.Find(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                if (Request.Form.Files.Count>0)
                {
                    var filesId = Domain.File.Upload("UserImages", _fileRepository, Request.Form.Files);
                    user.Image = filesId[0];
                }
                _userRepository.UserUpdate(user);
                SessionClass.SetUser(HttpContext,user);
                Success(Resource.AlertDataSavedSuccessfully);
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("Profile");
        }
        [Authrize("EndUser")]
        public IActionResult CompleteRegister(Guid id)
        {
            var model = new UserAccountViewModel();
            model.User = _userRepository.Find(id);
            return View(model);
        }


        [HttpPost]
        [Authrize("EndUser")]
        public IActionResult CompleteRegister(UserAccountViewModel model)
        {
            try
            {
                var user = _userRepository.Find(model.User.Id);
                user.IsConfirmed = true;
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.CityId = model.User.CityId;
                user.Region = model.User.Region;
                user.WhatsAppNumber = model.User.WhatsAppNumber;
                user.Email = model.User.Email;
                _userRepository.UserUpdate(user);
                SessionClass.SetUser(HttpContext, user);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Authrize("EndUser")]
        public IActionResult ContactUs()
        {
            return View(new ContactUs());
        }
        [HttpPost]
        [Authrize("EndUser")]
        public IActionResult ContactUs(ContactUs model)
        {
            try
            {
                _contactUsRepository.Add(model);
                Success(Resource.MessageHasBeenSent);
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
            return View(model);
        }
        private string getCode()
        {
            Random generator = new Random();
            return generator.Next(999999, 99999999).ToString("D6").Substring(0, 6);
        }
    }
}
