using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Controllers.AdminControl
{
    public class SettingsController : BaseController
    {
        private readonly IRepository<Setting> _settingRepository;
        public SettingsController(IRepository<Setting> settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public IActionResult Index()
        {
            var settings = _settingRepository.Table.ToList();
            return View(settings);
        }
        [HttpPost]
        public IActionResult SaveSetting(string AppName, string Copyrights,string CompanyPhoneNumber,string CompanyEmail,string NOTICE
            ,string ReservationPhoneNumber,string CustomerServiceFrom,string CustomerServiceTo,string ContactPhone,string OfficeAddress,
            string PleaseVisitOurOfficeToPay)
        {
            try
            {
                List<Setting> settings = new List<Setting>();
                SetSittingValue(nameof(Copyrights), Copyrights);
                SetSittingValue(nameof(AppName), AppName);
                SetSittingValue(nameof(CompanyPhoneNumber), CompanyPhoneNumber);
                SetSittingValue(nameof(CompanyEmail), CompanyEmail);
                SetSittingValue(nameof(NOTICE), NOTICE);
                SetSittingValue(nameof(ReservationPhoneNumber), ReservationPhoneNumber);
                SetSittingValue(nameof(CustomerServiceFrom), CustomerServiceFrom);
                SetSittingValue(nameof(CustomerServiceTo), CustomerServiceTo);
                SetSittingValue(nameof(ContactPhone), ContactPhone);
                SetSittingValue(nameof(OfficeAddress), OfficeAddress);
                SetSittingValue(nameof(PleaseVisitOurOfficeToPay), PleaseVisitOurOfficeToPay);
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        private void SetSittingValue(string key,string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var setting = _settingRepository.Table.FirstOrDefault(c => c.Key == key);
                if (setting==null)
                {
                    setting = new Setting();
                    setting.Key = key;
                    setting.Value = value;
                    _settingRepository.Add(setting);
                }
                else
                {
                    setting.Value = value;
                    _settingRepository.Update(setting);
                }
            }
        }
    }
}
