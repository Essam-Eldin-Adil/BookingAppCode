using Data.Models;
using Data.ViewModels;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public AccountsController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
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
            return generator.Next(1, 1000000).ToString("D6");
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
            return RedirectToAction(redirectUrl);
        }

        private IActionResult redirectUser(User user)
        {
            switch (user.UserType)
            {
                case (int)Enums.UserType.BookAdmin:
                    return RedirectToAction("Index","UserAccount");
                case (int)Enums.UserType.Admin:
                    return RedirectToAction("Index", "Administrator");
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
