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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using iQuarc.DataLocalization;
using File = Data.Models.File;
using Data.Models.Chalets.ChaletDetails;
using static BookingApp.Authrize;

namespace BookingApp.Controllers
{
    [Authrize("Admin,BookAdmin,BookUser")]
    public class UserAccountController : BaseController
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<UnitImage> _unitImagesRepository;
        private readonly IRepository<ChaletUser> _chaletUserRepository;
        private readonly IRepository<ChaletImage> _chaletImageRepository;
        private readonly IRepository<Chalet> _chaletRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<ChaletBank> _chaletBankRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<Job> _jobRepository;
        private readonly IRepository<ResortParameterValue> _resortParameterValueRepository;
        private readonly IRepository<ChaletParameterValue> _chaletParameterValueRepository;
        private readonly IRepository<ParameterGroup> _groupRepository;
        private readonly IRepository<PricePerDay> _pricePerDayRepository;
        private readonly IRepository<Offer> _offerRepository;

        public UserAccountController(IRepository<User> userRepository, IRepository<ChaletUser> chaletUserRepository,
            IRepository<ParameterGroup> groupRepository,
            IRepository<Unit> unitRepository,
                        IRepository<UnitImage> unitImagesRepository,
IRepository<PricePerDay> pricePerDayRepository,
            IRepository<Offer> offerRepository,
            IRepository<ChaletParameterValue> chaletParameterValueRepository,
            IRepository<ChaletImage> chaletImageRepository, IRepository<Chalet> chaletRepository, IRepository<ResortParameterValue> resortParameterValueRepository,
            IRepository<File> fileRepository, IRepository<City> cityRepository, IRepository<ChaletBank> chaletBankRepository, IRepository<Bank> bankRepository, IRepository<Job> jobRepository)
        {
            _unitImagesRepository = unitImagesRepository;
            _userRepository = userRepository;
            _chaletUserRepository = chaletUserRepository;
            _chaletImageRepository = chaletImageRepository;
            _chaletRepository = chaletRepository;
            _fileRepository = fileRepository;
            _chaletBankRepository = chaletBankRepository;
            _bankRepository = bankRepository;
            _jobRepository = jobRepository;
            _cityRepository = cityRepository;
            _pricePerDayRepository = pricePerDayRepository;
            _offerRepository = offerRepository;
            _resortParameterValueRepository = resortParameterValueRepository;
            _groupRepository = groupRepository;
            _unitRepository = unitRepository;
            _chaletParameterValueRepository = chaletParameterValueRepository;
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
                var chaletUsers = await _chaletUserRepository.Table.Include("Chalet.Units.UnitImages.File").Include(c => c.Chalet.City).Include("Chalet.ChaletImages.File").Where(c=>c.UserId==user.Id).ToListAsync();
                
                return View(chaletUsers);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Chalet(Guid? id,int? type)
        {
            try
            {
                var model = new ChaletViewModel();
                ViewBag.New = false;
                model.Chalet = _chaletRepository.Table.Include("ChaletImages.File").FirstOrDefault(c => c.Id == id);
                var cities = _cityRepository.Table.Select(c => new
                {
                    Id = c.Id,
                    CityName = c.CityName
                }).Localize(new CultureInfo(Lang)).ToList();
                ViewBag.Cities = new SelectList(cities, "Id", "CityName");

                if (model.Chalet == null)
                {
                    model.Chalet = new Chalet();
                    ViewBag.New = true;
                    if (type == null)
                    {
                        type = (int)Enums.PropertyType.Resort;
                    }
                    else if (type != (int)Enums.PropertyType.Resort
                        &&type != (int)Enums.PropertyType.Villa
                        && type != (int)Enums.PropertyType.Reset)
                    {
                        type = (int)Enums.PropertyType.Resort;
                    }
                    model.Chalet.PropertyType = (int)type;
                    return View(model);
                }
                else
                {
                    model.ChaletUsers = _chaletUserRepository.Table.Include(c => c.User).Where(c => c.ChaletId == model.Chalet.Id).ToList();

                    if (model.Chalet.PropertyType == (int)Enums.PropertyType.Resort)
                    {
                        model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.MainResort || c.PropertyType == (int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                        model.ResortParameterValues = _resortParameterValueRepository.Table.Where(c => c.ChaletId == model.Chalet.Id).ToList();
                    }
                    else if(model.Chalet.PropertyType == (int)Enums.PropertyType.Reset)
                    {
                        model.Unit = _unitRepository.Table.Include("UnitImages.File").FirstOrDefault(c => c.ChaletId == model.Chalet.Id);
                        model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.Reset || c.PropertyType == (int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                        if (model.Unit != null)
                        {
                            model.ChaletParameterValues = _chaletParameterValueRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();

                            model.Offers = _offerRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();
                            model.PricePerDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == model.Unit.Id);
                            if (model.PricePerDay == null)
                            {
                                model.PricePerDay = new PricePerDay();
                            }
                        }
                        else
                        {
                            model.Unit = new Unit();
                            model.Offers = new List<Offer>();
                            model.PricePerDay = new PricePerDay();
                        }
                    }else if (model.Chalet.PropertyType == (int)Enums.PropertyType.Villa)
                    {
                        model.Unit = _unitRepository.Table.Include("UnitImages.File").FirstOrDefault(c => c.ChaletId == model.Chalet.Id);
                        model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.Villa || c.PropertyType == (int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                        if (model.Unit != null)
                        {
                            model.ChaletParameterValues = _chaletParameterValueRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();

                            model.Offers = _offerRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();
                            model.PricePerDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == model.Unit.Id);
                            if (model.PricePerDay == null)
                            {
                                model.PricePerDay = new PricePerDay();
                            }
                        }
                        else
                        {
                            model.Unit = new Unit();
                            model.Offers = new List<Offer>();
                            model.PricePerDay = new PricePerDay();
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
        public IActionResult GetProprityFeilds(int proprityType,Guid? chaletId)
        {
            var model = new ChaletViewModel();
            var cities = _cityRepository.Table.Select(c => new
            {
                Id = c.Id,
                CityName = c.CityName
            }).Localize(new CultureInfo(Lang)).ToList();
            ViewBag.Cities = new SelectList(cities, "Id", "CityName");


            if (chaletId!=null||chaletId==Guid.Empty)
            {
                model.Chalet = _chaletRepository.Table.Include("ChaletImages.File").FirstOrDefault(c=>c.Id==chaletId);
                if (model.Chalet==null)
                {
                    model.Chalet = new Chalet();
                }
                else
                {
                    model.ResortParameterValues = _resortParameterValueRepository.Table.Where(c => c.ChaletId == model.Chalet.Id).ToList();
                    model.ChaletUsers = _chaletUserRepository.Table.Include(c => c.User).Where(c => c.ChaletId == chaletId).ToList();
                }
            }
            var partialView = "";
            switch (proprityType)
            {
                case (int)Enums.PropertyType.Reset:
                    partialView = "_reset";
                    model.Unit = _unitRepository.Table.Include("UnitImages.File").FirstOrDefault(c => c.ChaletId == model.Chalet.Id);
                    model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.Reset || c.PropertyType == (int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                    if (model.Unit!=null)
                    {
                        model.ChaletParameterValues = _chaletParameterValueRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();

                        model.Offers = _offerRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();
                        model.PricePerDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == model.Unit.Id);
                    }
                    else
                    {
                        model.Unit = new Unit();
                        model.Offers = new List<Offer>();
                        model.PricePerDay = new PricePerDay();
                    }
                    break;
                case (int)Enums.PropertyType.Villa:
                    model.Unit = _unitRepository.Table.Include("UnitImages.File").FirstOrDefault(c => c.ChaletId == model.Chalet.Id); 
                    model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.Villa || c.PropertyType == (int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                    partialView = "_villa";
                    if (model.Unit!=null)
                    {
                        model.ChaletParameterValues = _chaletParameterValueRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();

                        model.Offers = _offerRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();
                        model.PricePerDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == model.Unit.Id);
                    }
                    else
                    {
                        model.Unit = new Unit();
                        model.Offers = new List<Offer>();
                        model.PricePerDay = new PricePerDay();
                    }
                    break;
                case (int)Enums.PropertyType.Resort:
                    model.ParameterGroups = _groupRepository.Table.Where(c => c.PropertyType == (int)Enums.PropertyType.MainResort||c.PropertyType==(int)Enums.PropertyType.All).Include(c => c.Parameters).ToList();
                    model.ResortParameterValues = _resortParameterValueRepository.Table.Where(c => c.ChaletId == model.Chalet.Id).ToList();
                    partialView = "_resort";
                    break;
            }
            

            return PartialView(partialView, model);
        }


        [HttpPost]
        public IActionResult SaveChalet(ChaletViewModel model)
        {
            try
            {
                var chalet = _chaletRepository.Find(model.Chalet.Id);
                if (chalet == null)
                {
                    _chaletRepository.Add(model.Chalet);
                    chalet = model.Chalet;
                    var user = SessionClass.GetUser(HttpContext);
                    var chaletUser = new ChaletUser();
                    chaletUser.UserId = user.Id;
                    chaletUser.ChaletId = model.Chalet.Id;
                    chaletUser.IsAdmin = true;
                    _chaletUserRepository.Add(chaletUser);
                }
                else
                {
                    chalet.ProprityCount = model.Chalet.ProprityCount;
                    chalet.CloseToSea = model.Chalet.CloseToSea;
                    chalet.DistanceFromSea = model.Chalet.DistanceFromSea;
                    chalet.ViewStatus = model.Chalet.ViewStatus;
                    chalet.Longitude = model.Chalet.Longitude;
                    chalet.Latitude = model.Chalet.Latitude;
                    chalet.Location = model.Chalet.Location;
                    chalet.ChaletName = model.Chalet.ChaletName;
                    chalet.CityId = model.Chalet.CityId;
                    chalet.Region = model.Chalet.Region;
                    chalet.Neighborhood = model.Chalet.Neighborhood;
                    chalet.Description = model.Chalet.Description;
                    chalet.Direction = model.Chalet.Direction;
                    chalet.PropertyType = model.Chalet.PropertyType;
                    _chaletRepository.Update(chalet);
                    model.Chalet = chalet;
                }
                if (model.Chalet.PropertyType != (int)Enums.PropertyType.Resort)
                {
                    var unit = _unitRepository.Table.FirstOrDefault(c => c.ChaletId == chalet.Id);
                    if (unit == null)
                    {
                        unit = new Unit();
                        unit.Description = model.Chalet.Description;
                        unit.MaximumAllowed = model.Unit.MaximumAllowed;
                        unit.ViewStatus = model.Chalet.ViewStatus;
                        unit.MoreThanAllowed = model.Unit.MoreThanAllowed;
                        unit.MoreThanAllowedPrice = model.Unit.MoreThanAllowedPrice;
                        unit.AllowedPersons = model.Unit.AllowedPersons;
                        unit.Name = chalet.ChaletName;
                        unit.IsDayPrice = model.Unit.IsDayPrice;
                        unit.DayPrice = model.Unit.DayPrice;
                        unit.IsSimilar = false;
                        unit.DepositAmount = model.Unit.DepositAmount;
                        unit.ChaletId = chalet.Id;
                        unit.Space = model.Unit.Space;
                        _unitRepository.Add(unit);
                    }
                    else
                    {
                        unit.MaximumAllowed = model.Unit.MaximumAllowed;
                        unit.Description = model.Chalet.Description;
                        unit.MoreThanAllowed = model.Unit.MoreThanAllowed;
                        unit.ViewStatus = model.Chalet.ViewStatus;
                        unit.MoreThanAllowedPrice = model.Unit.MoreThanAllowedPrice;
                        unit.AllowedPersons = model.Unit.AllowedPersons;
                        unit.Name = chalet.ChaletName;
                        unit.IsSimilar = false;
                        unit.IsDayPrice = model.Unit.IsDayPrice;
                        unit.DepositAmount = model.Unit.DepositAmount;
                        unit.Space = model.Unit.Space;
                        unit.DayPrice = model.Unit.DayPrice;
                        _unitRepository.Update(unit);
                    }
                    if (model.Unit.IsDayPrice)
                    {
                        var perDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == unit.Id);
                        if (perDay == null)
                        {
                            perDay = new PricePerDay();
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
                    }
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet",new { id=model.Chalet.Id});
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveChaletImages(Guid ChaletId)
        {
            try
            {
                var isHasImages = _chaletRepository.Table.Any(c => c.Id == ChaletId);
                var files = Request.Form.Files;
                var filesId = Domain.File.Upload("ChaletImages", _fileRepository, files);
                var chaletImages = new List<ChaletImage>();
                foreach (var id in filesId)
                {
                    var chaletImage = new ChaletImage();
                    chaletImage.ChaletId = ChaletId;
                    chaletImage.FileId = id;
                    if (!isHasImages)
                    {
                        if (filesId.IndexOf(id) == 0)
                        {
                            chaletImage.IsPrimary = true;
                        }
                    }
                    chaletImages.Add(chaletImage);
                }
                _chaletImageRepository.AddRange(chaletImages.AsEnumerable());
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet", new { id = ChaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveUnitImages(Guid ChaletId)
        {
            try
            {
                var unit = _unitRepository.Table.FirstOrDefault(c => c.ChaletId == ChaletId);
                var isHasImages = _unitImagesRepository.Table.Any(c => c.UnitId == unit.Id);
                var files = Request.Form.Files;
                var filesId = Domain.File.Upload("UnitImages", _fileRepository, files);
                var unitImages = new List<UnitImage>();
                foreach (var id in filesId)
                {
                    var unitImage = new UnitImage();
                    unitImage.UnitId = unit.Id;
                    unitImage.FileId = id;
                    if (!isHasImages)
                    {
                        if (filesId.IndexOf(id) == 0)
                        {
                            unitImage.IsPrimary = true;
                        }
                    }
                    unitImages.Add(unitImage);
                }
                _unitImagesRepository.AddRange(unitImages.AsEnumerable());
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet", new { id = ChaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult SaveUtilities(Guid ChaletId,bool isUnit, string[] inputValues, Guid[] inputIds, Guid[] checkboxes)
        {
            try
            {
                if (isUnit)
                {
                    var unit = _unitRepository.Table.Where(c => c.ChaletId == ChaletId).FirstOrDefault();
                    BindUnitParameter(inputValues, inputIds, checkboxes, unit.Id);
                }
                else
                {
                    BindParameter(inputValues, inputIds, checkboxes, ChaletId);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet", new { id = ChaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[HttpPost]
        //public IActionResult SaveChalet(ChaletViewModel model, string[] inputValues, Guid[] inputIds, Guid[] checkboxes)
        //{
        //    try
        //    {
        //        var isHasImages = false;
        //        var chalet = _chaletRepository.Find(model.Chalet.Id);
        //        if (chalet == null)
        //        {
        //            _chaletRepository.Add(model.Chalet);
        //            chalet = model.Chalet;
        //            var user = SessionClass.GetUser(HttpContext);
        //            var chaletUser = new ChaletUser();
        //            chaletUser.UserId = user.Id;
        //            chaletUser.ChaletId = model.Chalet.Id;
        //            chaletUser.IsAdmin = true;
        //            _chaletUserRepository.Add(chaletUser);
        //        }
        //        else
        //        {
        //            chalet.ProprityCount = model.Chalet.ProprityCount;
        //            chalet.CloseToSea = model.Chalet.CloseToSea;
        //            chalet.DistanceFromSea = model.Chalet.DistanceFromSea;
        //            chalet.ViewStatus = model.Chalet.ViewStatus;
        //            chalet.Longitude = model.Chalet.Longitude;
        //            chalet.Latitude = model.Chalet.Latitude;
        //            chalet.Location = model.Chalet.Location;
        //            chalet.ChaletName = model.Chalet.ChaletName;
        //            chalet.CityId = model.Chalet.CityId;
        //            chalet.Region = model.Chalet.Region;
        //            chalet.Neighborhood = model.Chalet.Neighborhood;
        //            chalet.Description = model.Chalet.Description;
        //            chalet.Direction = model.Chalet.Direction;
        //            chalet.PropertyType = model.Chalet.PropertyType;
        //            _chaletRepository.Update(chalet);
        //        }
        //        if (model.Chalet.PropertyType!=(int)Enums.PropertyType.Resort)
        //        {
        //            var unit = _unitRepository.Table.FirstOrDefault(c=>c.ChaletId == chalet.Id);
        //            if (unit==null)
        //            {
        //                unit = new Unit();
        //                unit.MaximumAllowed = model.Unit.MaximumAllowed;
        //                unit.MoreThanAllowed = model.Unit.MoreThanAllowed;
        //                unit.MoreThanAllowedPrice = model.Unit.MoreThanAllowedPrice;
        //                unit.AllowedPersons = model.Unit.AllowedPersons;
        //                unit.Name = chalet.ChaletName;
        //                unit.IsDayPrice = model.Unit.IsDayPrice;
        //                unit.DayPrice = model.Unit.DayPrice;
        //                unit.IsSimilar = false;
        //                unit.DepositAmount = model.Unit.DepositAmount;
        //                unit.ChaletId = chalet.Id;
        //                unit.Space = model.Unit.Space;
        //                _unitRepository.Add(unit);
        //                BindUnitParameter(inputValues, inputIds, checkboxes, unit.Id);
        //            }
        //            else
        //            {
        //                unit.MaximumAllowed = model.Unit.MaximumAllowed;
        //                unit.MoreThanAllowed = model.Unit.MoreThanAllowed;
        //                unit.MoreThanAllowedPrice = model.Unit.MoreThanAllowedPrice;
        //                unit.AllowedPersons = model.Unit.AllowedPersons;
        //                unit.Name = chalet.ChaletName;
        //                unit.IsSimilar = false;
        //                unit.IsDayPrice = model.Unit.IsDayPrice;
        //                unit.DepositAmount = model.Unit.DepositAmount;
        //                unit.Space = model.Unit.Space;
        //                unit.DayPrice = model.Unit.DayPrice;
        //                _unitRepository.Update(unit);
        //                BindUnitParameter(inputValues, inputIds, checkboxes, unit.Id);
        //            }
        //            if (model.Unit.IsDayPrice)
        //            {
        //                var perDay = _pricePerDayRepository.Table.FirstOrDefault(c => c.UnitId == unit.Id);
        //                if (perDay == null)
        //                {
        //                    perDay = new PricePerDay();
        //                    _pricePerDayRepository.Add(model.PricePerDay);
        //                }
        //                else
        //                {
        //                    perDay.Friday = model.PricePerDay.Friday;
        //                    perDay.Monday = model.PricePerDay.Monday;
        //                    perDay.Saturday = model.PricePerDay.Saturday;
        //                    perDay.Sunday = model.PricePerDay.Sunday;
        //                    perDay.Thursday = model.PricePerDay.Thursday;
        //                    perDay.Wednesday = model.PricePerDay.Wednesday;
        //                    perDay.Tuesday = model.PricePerDay.Tuesday;
        //                    _pricePerDayRepository.Update(perDay);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            BindParameter(inputValues, inputIds, checkboxes, chalet.Id);
        //        }
        //        if (chalet.PropertyType==(int)Enums.PropertyType.Resort)
        //        {
        //            isHasImages = _unitRepository.Table.Any(c => c.ChaletId == model.Chalet.Id);
        //            var files = Request.Form.Files;
        //            var filesId = Domain.File.Upload("ChaletImages", _fileRepository, files);
        //            foreach (var id in filesId)
        //            {
        //                var chaletImage = new ChaletImage();
        //                chaletImage.ChaletId = model.Chalet.Id;
        //                chaletImage.FileId = id;
        //                if (!isHasImages)
        //                {
        //                    if (filesId.IndexOf(id) == 0)
        //                    {
        //                        chaletImage.IsPrimary = true;
        //                    }
        //                }
        //                _chaletImageRepository.Add(chaletImage);
        //            }
        //        }
        //        else
        //        {
        //            var unit = _unitRepository.Table.FirstOrDefault(c => c.ChaletId == chalet.Id);
        //            isHasImages = _unitImagesRepository.Table.Any(c => c.UnitId == unit.Id);
        //            var files = Request.Form.Files;
        //            var filesId = Domain.File.Upload("UnitImages", _fileRepository, files);
        //            foreach (var id in filesId)
        //            {
        //                var unitImage = new UnitImage();
        //                unitImage.UnitId = unit.Id;
        //                unitImage.FileId = id;
        //                if (!isHasImages)
        //                {
        //                    if (filesId.IndexOf(id) == 0)
        //                    {
        //                        unitImage.IsPrimary = true;
        //                    }
        //                }
        //                _unitImagesRepository.Add(unitImage);
        //            }
        //        }


        //        Success(Resource.AlertDataSavedSuccessfully);
        //        return RedirectToAction("Chalets");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        private void BindUnitParameter(IReadOnlyList<string> inputValues, IReadOnlyList<Guid> inputIds, IEnumerable<Guid> checkboxes, Guid unitId)
        {
            var unitParameters = _chaletParameterValueRepository.Table.Where(c => c.UnitId == unitId).ToList();
            _chaletParameterValueRepository.RemoveHardRange(unitParameters);

            for (var i = 0; i < inputIds.Count; i++)
            {
                var paramValues = new ChaletParameterValue
                {
                    UnitId = unitId,
                    ParameterId = inputIds[i],
                    Value = inputValues[i]
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


        private void BindParameter(IReadOnlyList<string> inputValues, IReadOnlyList<Guid> inputIds, IEnumerable<Guid> checkboxes, Guid chaletId)
        {
            var unitParameters = _resortParameterValueRepository.Table.Where(c => c.ChaletId == chaletId).ToList();
            _resortParameterValueRepository.RemoveHardRange(unitParameters);

            for (var i = 0; i < inputIds.Count; i++)
            {
                var paramValues = new ResortParameterValue
                {
                    ChaletId = chaletId,
                    ParameterId = inputIds[i],
                    Value = inputValues[i]
                };
                _resortParameterValueRepository.Add(paramValues);
            }

            foreach (var checkbox in checkboxes)
            {
                var paramValues = new ResortParameterValue
                {
                    ChaletId = chaletId,
                    ParameterId = checkbox,
                    Value = "1"
                };
                _resortParameterValueRepository.Add(paramValues);
            }

        }

        [HttpPost]
        public IActionResult AddOffer(UnitViewModel model, Guid chaletId)
        {
            try
            {
                _offerRepository.Add(model.Offer);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting", new { id = chaletId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpGet]
        public IActionResult RemoveOffer(Guid id, Guid unitId, Guid chaletId)
        {
            try
            {
                var offer = _offerRepository.Find(id);
                _offerRepository.Remove(offer);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting", new { id =  chaletId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                model.Chalet = _chaletRepository.Find(id);
                model.ChaletBank.ChaletId = id;
                
                model.ChaletBanks = _chaletBankRepository.Table.Include(c=>c.Bank).Where(c => c.ChaletId == id).ToList();
                var banks= _bankRepository.Table.Select(c => new {c.Name,c.Id,c.Image}).Localize(new CultureInfo(Lang)).ToList();
                //if (!model.Chalet.IsConfirmed)
                //{
                //    Warning(Resource.ProrpertNotConfirmed);
                //}
                ViewBag.Banks = new SelectList(banks,"Id","Name");
                ViewBag.Jobs = new SelectList(await _jobRepository.ToListAsync(),"Id","Name");
                if (model.Chalet.PropertyType == (int)Enums.PropertyType.Resort)
                {
                    ViewBag.IsResort = model.Chalet.Id;
                }
                model.Unit = _unitRepository.Table.FirstOrDefault(c => c.ChaletId == id);
                if (model.Unit!=null)
                {
                    model.Offers = _offerRepository.Table.Where(c => c.UnitId == model.Unit.Id).ToList();
                }
                else
                {
                    model.Unit = new Unit();
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
                    _userRepository.UserUpdate(userInDb);
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

            return RedirectToAction("Chalet", new {id = chaletId });
        }

        [HttpPost]
        public IActionResult SaveSetting(Chalet model)
        {
            try
            {
                var setting = _chaletRepository.Find(model.Id);
                setting.EnterTime = model.EnterTime;
                setting.ExitTime = model.ExitTime;
                setting.CleanCondition = model.CleanCondition;
                setting.InsuranceCondition = model.InsuranceCondition;
                setting.OtherCondition = model.OtherCondition;
                setting.FamilyCondition = model.FamilyCondition;
                setting.InsuranceAmount = model.InsuranceAmount;
                setting.ReservationManager = model.ReservationManager;
                setting.ReservationPhoneNumber = model.ReservationPhoneNumber;
                _chaletRepository.Update(setting);


                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Setting",new {id=model.Id});
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
        [HttpGet]
        public IActionResult SetDefaultChaletImage(Guid id, Guid chaletId)
        {
            try
            {

                var chaletImage = _chaletImageRepository.Table.Where(c => c.ChaletId == chaletId);
                foreach (var item in chaletImage)
                {
                    if (item.Id == id)
                    {
                        item.IsPrimary = true;
                    }
                    else
                    {
                        item.IsPrimary = false;
                    }
                }
                _chaletImageRepository.UpdateRange(chaletImage);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet", new { id = chaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult SetDefaultUnitImage(Guid id,Guid unitId,Guid chaletId)
        {
            try
            {
                
                var unitImage = _unitImagesRepository.Table.Where(c=>c.UnitId== unitId);
                foreach (var item in unitImage)
                {
                    if (item.Id==id)
                    {
                        item.IsPrimary = true;
                    }
                    else
                    {
                        item.IsPrimary = false;
                    }
                }
                _unitImagesRepository.UpdateRange(unitImage);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction("Chalet", new { id = chaletId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
