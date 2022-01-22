using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models.Chalets;
using Data.Models.General;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace BookingApp.Controllers.AdminControl
{
    public class BanksController : BaseController
    {
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<BankTranslation> _bankTranslationRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<File> _fileRepository;
        public BanksController(IRepository<Bank> bankRepository, IRepository<BankTranslation> bankTranslationRepository, IRepository<Language> languageRepository, IRepository<File> fileRepository)
        {
            _bankRepository = bankRepository;
            _bankTranslationRepository = bankTranslationRepository;
            _languageRepository = languageRepository;
            _fileRepository = fileRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult<IEnumerable<Bank>> GetData()
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
                var data = _bankRepository.Table.Where(c => (string.IsNullOrEmpty(search) || c.Name == search)).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList();
                foreach (var bank in data)
                {
                    bank.ImageUrl = Url.Content("~/" + Domain.File.GetImage(HttpContext, bank.Image));
                }
                return Ok(new { draw = draw, data, recordsTotal = _bankRepository.Table.Count(), recordsFiltered = _bankRepository.Table.Count() });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult Bank(Guid? id)
        {
            var model = new BankViewModel();
            try
            {
                ViewBag.Languages = new SelectList(_languageRepository.Table.ToList(),"Id", "Name");
                if (id!=null)
                {
                    model.Bank = _bankRepository.Table.Include("BankTranslations.Language").FirstOrDefault(c => c.Id == id);
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
        public IActionResult SaveBank(BankViewModel model)
        {
            try
            {
                var bank = _bankRepository.Find(model.Bank.Id);
                if (bank == null)
                {
                    bank = new Bank() {Name = model.Bank.Name};
                    _bankRepository.Add(bank);
                }
                else
                {
                    bank.Name = model.Bank.Name;
                    _bankRepository.Update(bank);
                }
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    if (bank.Image != Guid.Empty && bank.Image != null)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)bank.Image);
                    }
                    var filesId = Domain.File.Upload("BankImages", _fileRepository, files);
                    foreach (var guid in filesId)
                    {
                        bank.Image = guid;
                        _bankRepository.Update(bank);
                    }
                }
                
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Bank),new {id=model.Bank.Id});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        [HttpPost]
        public IActionResult SaveBankTranslation(BankViewModel model)
        {
            try
            {
                var bank = _bankTranslationRepository.Find(model.BankTranslation.Id);
                if (bank == null)
                {
                    _bankTranslationRepository.Add(model.BankTranslation);
                }
                else
                {
                    bank.Name = model.BankTranslation.Name;
                    bank.LanguageId = model.BankTranslation.LanguageId;
                    _bankTranslationRepository.Update(bank);
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Bank), new { id = model.BankTranslation.BankId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        public IActionResult RemoveBank(Guid id)
        {
            try
            {
                var bank = _bankRepository.Find(id);
                if (bank != null)
                {
                    _bankRepository.Remove(bank);
                    if (bank.Image == Guid.Empty)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)bank.Image);
                    }
                }
                Success(Resource.AlertDataSavedSuccessfully);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public IActionResult RemoveBankTranslation(Guid id)
        {
            try
            {
                var bankTranslation = _bankTranslationRepository.Find(id);
                if (bankTranslation != null)
                {
                    _bankTranslationRepository.Remove(bankTranslation);
                }
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
