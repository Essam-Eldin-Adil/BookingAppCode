using Data.Models;
using Data.Models.General;
using Data.ViewModels;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<City> _cityRepository;
        public AccountsController(IRepository<User> userRepository, IRepository<City> cityRepository)
        {
            _userRepository = userRepository;
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            SessionClass.Remove(HttpContext);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Login()
        {
            var model = new LoginViewModel();
            ViewBag.CodeSent = false;
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(string mobileno, int code, bool confirm = false)
        {
            var user = _userRepository.Table.FirstOrDefault(c => c.PhoneNumber == mobileno);
            if (user != null)
            {
                if (user.UserType == (int)Enums.UserType.EndUser)
                {
                    ViewBag.IsExist = false;
                    ViewBag.CodeSent = false;
                    ViewBag.MobileNo = mobileno;
                    Error(Resource.YouCannotLoginWithEndUserAccount);
                    return View();
                }
            }
            if (confirm)
            {
                if (user.ConfirmCode == code)
                {
                    SessionClass.SetUser(HttpContext, user);
                    if (user.IsConfirmed)
                    {
                        return RedirectToAction("Chalets", "UserAccount");
                    }

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
            if (user == null)
            {
                user = new User();
                user.UserType = (int)Enums.UserType.BookAdmin;
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

        public IActionResult CompleteRegister(Guid id)
        {
            var model = new UserAccountViewModel();
            model.User = _userRepository.Find(id);
            ViewBag.CityId = new SelectList(_cityRepository.Table.ToList(),"Id", "CityName");
            return View(model);
        }

        [HttpPost]
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
                Success(Resource.ProrpertNotConfirmed);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public IActionResult Login1(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Error(Resource.Validations);
                    return View(model);
                }
                
                if (!model.isPassword)
                {
                    var user = _userRepository.Table.FirstOrDefault(c => c.Email == model.Email);
                    if (user==null)
                    {
                        model.isRegistration = true;
                    }
                    else
                    {
                        model.isPassword = true;
                    }
                    model.isPostback = true;
                    return View(model);
                }
                else
                {
                    var user = _userRepository.Table.FirstOrDefault(c => c.Email == model.Email&&c.Password==model.Password);
                    if (user==null)
                    {
                        model = new LoginViewModel();
                        Error(Resource.InvalidLoginAttempt);
                        model.isPostback = true;
                        return View(model);
                    }
                    else
                    {
                        SessionClass.SetUser(HttpContext, user);
                        return redirectUser(user);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            try
            {
                string code = getCode();
                if (!string.IsNullOrEmpty(model.Email))
                {
                    var user = _userRepository.Table.FirstOrDefault(c => c.Email ==model.Email);
                    if (user!=null)
                    {
                        Error(Resource.UserAlreadyExists);
                        return RedirectToAction(nameof(Login));
                    }
                    user = new User();
                    user.PhoneNumber = model.PhoneNumber;
                    user.Password = model.Password;
                    user.UserType = model.UserType;
                    user.Email = model.Email;
                    user.ConfirmCode = long.Parse(code);
                    await _userRepository.AddAsync(user);
                    return RedirectToAction(nameof(Confirm),new { id= user.Id });
                }
                else
                {
                    Error(Resource.Validations);
                    return RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string getCode()
        {
            Random generator = new Random();
            return generator.Next(999999, 99999999).ToString("D6").Substring(0, 6);
        }

        [HttpGet]
        public IActionResult Confirm(Guid id, string redirectUrl)
        {
            var model = _userRepository.Find(id);
            ViewBag.redirectUrl = redirectUrl;
            if (model==null)
            {
                return BadRequest();
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Confirm(Guid userId,int code,string redirectUrl)
        {
            var model = _userRepository.Table.FirstOrDefault(c=>c.Id==userId && c.ConfirmCode== code);
            if (model!=null)
            {
                model.IsConfirmed = true;
                await _userRepository.UpdateAsync(model);
                SessionClass.SetUser(HttpContext,model);
            }
            if (string.IsNullOrEmpty(redirectUrl))
            {
                
                return redirectUser(model);
            }
            return Redirect(redirectUrl);
        }

        private IActionResult redirectUser(User user)
        {
            switch (user.UserType)
            {
                case (int)Enums.UserType.BookAdmin:
                    return RedirectToAction("Chalets", "UserAccount");
                case (int)Enums.UserType.Admin:
                    return RedirectToAction("Index", "CPanel");
                case (int)Enums.UserType.EndUser:
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var model = _userRepository.Table.FirstOrDefault(c => c.Email == Email);
            if (model != null)
            {
                model.IsConfirmed = false;
                model.ConfirmCode = long.Parse(getCode());
                await _userRepository.UpdateAsync(model);
                return RedirectToAction("Confirm",new {id=model.Id, redirectUrl="ResetPassword" });
            }
            Error(Resource.WrongPhoneNumber);
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var user = new ResetPasswordViewModel();
            return View(user);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var user = SessionClass.GetUser(HttpContext);
            user.Password = model.Password;
            _userRepository.Update(user);
            Success(Resource.AlertDataSavedSuccessfully);
            return redirectUser(user);
        }
    }
}
