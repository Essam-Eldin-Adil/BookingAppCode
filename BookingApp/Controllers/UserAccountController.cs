using Data.Models;
using Data.Models.Chalets;
using Data.Models.General;
using Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using File = Data.Models.File;

namespace BookingApp.Controllers
{
    public class UserAccountController : BaseController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<ChaletUser> _chaletUserRepository;
        private readonly IRepository<ChaletImage> _chaletImageRepository;
        private readonly IRepository<Chalet> _chaletRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Neighborhood> _neighborhoodRepository;
        private readonly IRepository<ChaletBank> _chaletBankRepository;
        private readonly IRepository<ChaletSetting> _chaletSettingRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<Region> _regionRepository;
        private readonly IRepository<Job> _jobRepository;
        public UserAccountController(IRepository<User> userRepository, IRepository<ChaletUser> chaletUserRepository,
            IRepository<ChaletImage> chaletImageRepository, IRepository<Chalet> chaletRepository,
            IRepository<File> fileRepository, IRepository<City> cityRepository, IRepository<Neighborhood> neighborhoodRepository, IRepository<ChaletBank> chaletBankRepository, IRepository<ChaletSetting> chaletSettingRepository, IRepository<Bank> bankRepository, IRepository<Region> regionRepository, IRepository<Job> jobRepository)
        {
            _userRepository = userRepository;
            _chaletUserRepository = chaletUserRepository;
            _chaletImageRepository = chaletImageRepository;
            _chaletRepository = chaletRepository;
            _fileRepository = fileRepository;
            _neighborhoodRepository = neighborhoodRepository;
            _chaletBankRepository = chaletBankRepository;
            _chaletSettingRepository = chaletSettingRepository;
            _bankRepository = bankRepository;
            _regionRepository = regionRepository;
            _jobRepository = jobRepository;
            _cityRepository = cityRepository;
        }
        public IActionResult Index()
        {
            var model = new UserAccountViewModel();
            var user= SessionClass.GetUser(HttpContext);
            model.User = _userRepository.Find(user.Id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Chalets()
        {
            try
            {
                var user = SessionClass.GetUser(HttpContext);
                var chaletUsers = await _chaletUserRepository.Table.Include(c => c.Chalet.Units).Include(c => c.Chalet.City).Include("Chalet.ChaletImages.File").Where(c=>c.UserId==user.Id).ToListAsync();
                foreach (var chaletUser in chaletUsers.Where(chaletUser => !chaletUser.Chalet.IsConfirmed))
                {
                    Warning(Resource.ProrpertNotConfirmed);
                }
                return View(chaletUsers);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Chalet(Guid? id)
        {
            try
            {
                var model = new Chalet();
                ViewBag.Cities = new SelectList(_cityRepository.Table.ToList(),"Id", "CityName");
                ViewBag.Neighborhoods = new SelectList(_neighborhoodRepository.Table.ToList(),"Id", "Name");
                if (id==null)
                {
                    return View(model);
                }

                model = _chaletRepository.Table.Include("ChaletImages.File").FirstOrDefault(c => c.Id == id);
                return View(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult GetRegions(Guid cityId)
        {
            var regions = _regionRepository.Table.Where(c => c.CityId == cityId).ToList();
            return Json(regions);
        }
        public IActionResult GetNeighborhoods(Guid regionId)
        {
            var neighborhoods = _neighborhoodRepository.Table.Where(c => c.RegionId == regionId).ToList();
            return Json(neighborhoods);
        }

        [HttpPost]
        public IActionResult SaveChalet(Chalet model)
        {
            try
            {
                var isHasImages = false;
                var chalet = _chaletRepository.Find(model.Id);
                if (chalet == null)
                {
                    _chaletRepository.Add(model);
                    var user = SessionClass.GetUser(HttpContext);
                    var chaletUser = new ChaletUser();
                    chaletUser.UserId = user.Id;
                    chaletUser.ChaletId = model.Id;
                    chaletUser.IsAdmin = true;
                    _chaletUserRepository.Add(chaletUser);
                }
                    
                else
                {
                    chalet.ViewStatus = model.ViewStatus;
                    chalet.Longitude = model.Longitude;
                    chalet.Latitude = model.Latitude;
                    chalet.Location = model.Location;
                    chalet.ChaletName = model.ChaletName;
                    chalet.CityId = model.CityId;
                    chalet.RegionId = model.RegionId;
                    chalet.NeighborhoodId = model.NeighborhoodId;
                    chalet.Description = model.Description;
                    chalet.Direction = model.Direction;
                    chalet.PropertyType = model.PropertyType;
                    _chaletRepository.Update(chalet);
                }
                isHasImages = _chaletImageRepository.Table.Any(c => c.ChaletId == model.Id);
                var files = Request.Form.Files;
                var filesId = Domain.File.Upload("ChaletImages", _fileRepository, files);
                foreach (var id in filesId)
                {
                    var chaletImage = new ChaletImage();
                    chaletImage.ChaletId = model.Id;
                    chaletImage.FileId = id;
                    if (!isHasImages)
                    {
                        if (filesId.IndexOf(id) == 0)
                        {
                            chaletImage.IsPrimary = true;
                        }
                    }
                    _chaletImageRepository.Add(chaletImage);
                }
                
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet",new { id =model.Id});
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult RemoveChaletImage(Guid id,Guid chaletId)
        {
            Domain.File.Remove(_fileRepository, id);
            Success(Resource.AlertDataSavedSuccessfully);
            return RedirectToAction("Chalet", new { id = chaletId });
        }


        [HttpPost]
        public IActionResult SaveInfo(User model)
        {
            try
            {
                var user = _userRepository.Find(model.Id);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.BirthDate = model.BirthDate;
                _userRepository.Update(user);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Setting(Guid id)
        {
            var model = new ChaletSettingViewModel();
            try
            {
                model.ChaletSetting = _chaletSettingRepository.Table.FirstOrDefault(c => c.ChaletId == id) ?? new ChaletSetting {ChaletId = id};
                model.ChaletBank.ChaletId = id;
                model.ChaletBanks = _chaletBankRepository.Table.Include(c=>c.Bank).Where(c => c.ChaletId == id).ToList();
                model.ChaletUsers = _chaletUserRepository.Table.Include(c => c.User).Where(c => c.ChaletId == id).ToList();
                ViewBag.Banks = new SelectList(await _bankRepository.ToListAsync(),"Id","Name");
                ViewBag.Jobs = new SelectList(await _jobRepository.ToListAsync(),"Id","Name");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult SaveChaletUser(User user,Guid chaletId,bool notifications)
        {
            try
            {
                var userInDb = _userRepository.Find(user.Id);
                if (userInDb==null)
                {
                    user.TemporaryPassword = true;
                    user.IsConfirmed = true;
                    _userRepository.Add(user);
                    var chaletUser = new ChaletUser
                    {
                        IsAdmin = user.UserType==(int)Enums.UserType.BookAdmin?true:false,
                        ChaletId = chaletId,
                        UserId = user.Id,
                        SendWhatsAppNotifications = user.WhatsAppNotifications
                    };
                    _chaletUserRepository.Add(chaletUser);
                }
                else
                {
                    userInDb.UserType = user.UserType;
                    userInDb.PhoneNumber = user.PhoneNumber;
                    userInDb.WhatsAppNumber = user.WhatsAppNumber;
                    userInDb.FirstName = user.FirstName;
                    userInDb.LastName = user.LastName;
                    userInDb.JobId = user.JobId;
                    userInDb.Email = user.Email;
                    userInDb.Password = user.Password;
                    _userRepository.Update(userInDb);
                    var chaletUser = _chaletUserRepository.Table.FirstOrDefault(c => c.UserId == userInDb.Id);
                    if (chaletUser != null)
                    {
                        chaletUser.IsAdmin = user.UserType == (int) Enums.UserType.BookAdmin ? true : false;
                        chaletUser.SendWhatsAppNotifications = user.WhatsAppNotifications;
                        _chaletUserRepository.Update(chaletUser);
                    }
                }
                Success(Resource.AlertDataSavedSuccessfully);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("Setting", new {id = chaletId });
        }

        [HttpPost]
        public IActionResult SaveSetting(ChaletSetting model)
        {
            try
            {
                var setting = _chaletSettingRepository.Find(model.Id);
                if (setting==null)
                {
                    _chaletSettingRepository.Add(model);
                }
                else
                {
                    setting.EnterTime = model.EnterTime;
                    setting.ExitTime = model.ExitTime;
                    setting.CleanCondition = model.CleanCondition;
                    setting.InsuranceCondition = model.InsuranceCondition;
                    setting.OtherCondition = model.OtherCondition;
                    setting.FamilyCondition = model.FamilyCondition;
                    setting.InsuranceAmount = model.InsuranceAmount;
                    setting.ReservationManager = model.ReservationManager;
                    setting.ReservationPhoneNumber = model.ReservationPhoneNumber;
                    _chaletSettingRepository.Update(setting);
                }

                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting",new {id=model.ChaletId});
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveChaletBank(ChaletBank model)
        {
            try
            {
                var chaletBank = _chaletBankRepository.Find(model.Id);
                if (chaletBank == null)
                {
                    _chaletBankRepository.Add(model);
                }
                else
                {
                    chaletBank.AccountName = model.AccountName;
                    chaletBank.AccountNumber = model.AccountNumber;
                    chaletBank.BankId = model.BankId;
                    chaletBank.IBan = model.IBan;
                    _chaletBankRepository.Update(chaletBank);
                }

                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting", new { id = model.ChaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult RemoveChaletBank(Guid id)
        {
            try
            {
                
                var chaletBank = _chaletBankRepository.Find(id);
                Guid chaletId = chaletBank.ChaletId;
                _chaletBankRepository.Remove(chaletBank);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting", new { id = chaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
