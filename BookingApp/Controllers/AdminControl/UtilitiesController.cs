using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models.Chalets.ChaletDetails;
using Data.Models.General;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resources;

namespace BookingApp.Controllers.AdminControl
{
    public class UtilitiesController : BaseController
    {
        private readonly IRepository<ParameterGroup> _parameterGroupRepository;
        private readonly IRepository<ParameterGroupTranslation> _parameterGroupTranslationRepository;
        private readonly IRepository<Parameter> _parameterRepository;
        private readonly IRepository<ParameterTranslation> _parameterTranslationRepository;
        private readonly IRepository<File> _fileRepository;
        private readonly IRepository<Language> _languageRepository;

        public UtilitiesController(IRepository<ParameterGroup> parameterGroupRepository, IRepository<Parameter> parameterRepository, IRepository<File> fileRepository, IRepository<Language> languageRepository, IRepository<ParameterTranslation> parameterTranslationRepository, IRepository<ParameterGroupTranslation> parameterGroupTranslationRepository)
        {
            _parameterGroupRepository = parameterGroupRepository;
            _parameterRepository = parameterRepository;
            _fileRepository = fileRepository;
            _languageRepository = languageRepository;
            _parameterTranslationRepository = parameterTranslationRepository;
            _parameterGroupTranslationRepository = parameterGroupTranslationRepository;
        }

        public IActionResult Index()
        {
            UtilitiesViewModel model=new UtilitiesViewModel();
            try
            {
                ViewBag.Languages = new SelectList(_languageRepository.Table.ToList(), "Id", "Name");

                var param = _parameterGroupRepository.Table.Include("Parameters.ParameterTranslations").Include(c=>c.ParameterGroupTranslations).Where(c=>c.ParentId==Guid.Empty).ToList();
                foreach (var parameterGroup in param)
                {
                    var itemTree = new ItemTree {ParameterGroup = parameterGroup};
                    var nodes = _parameterGroupRepository.Table.Include("Parameters.ParameterTranslations").Include(c=>c.ParameterGroupTranslations).Where(c => c.ParentId == parameterGroup.Id).ToList();
                    itemTree.Type = parameterGroup.PropertyType;
                    itemTree.HaveNodes = nodes.Any();
                    if (itemTree.HaveNodes)
                    {
                        itemTree.ParameterGroups = nodes;
                    }
                    model.ItemTree.Add(itemTree);
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
        public IActionResult SaveParameter(UtilitiesViewModel model)
        {
            try
            {
                var parameter = _parameterRepository.Find(model.Parameter.Id);
                if (parameter == null)
                {
                    _parameterRepository.Add(model.Parameter);
                }
                else
                {
                    parameter.Index = model.Parameter.Index;
                    parameter.Name = model.Parameter.Name;
                    parameter.Type = model.Parameter.Type;
                    _parameterRepository.Update(parameter);
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
        [HttpPost]
        public IActionResult SaveSubGroup(UtilitiesViewModel model)
        {
            try
            {
                var parameterGroup = _parameterGroupRepository.Find(model.ParameterGroup.Id);
                if (parameterGroup == null)
                {
                    if (model.ParameterGroup.ParentId!=Guid.Empty)
                    {
                        model.ParameterGroup.IsChild = true;
                    }
                    _parameterGroupRepository.Add(model.ParameterGroup);
                }
                else
                {
                    parameterGroup.Filterable = model.ParameterGroup.Filterable;
                    parameterGroup.PropertyType = model.ParameterGroup.PropertyType;
                    parameterGroup.Name = model.ParameterGroup.Name;
                    parameterGroup.Order = model.ParameterGroup.Order;
                    _parameterGroupRepository.Update(parameterGroup);
                }
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    if (parameterGroup.Image!=Guid.Empty && parameterGroup.Image != null)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)parameterGroup.Image);
                    }
                    var filesId = Domain.File.Upload("Icons", _fileRepository, files);
                    foreach (var guid in filesId)
                    {
                        parameterGroup.Image = guid;
                        _parameterGroupRepository.Update(parameterGroup);
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

        public IActionResult RemoveGroup(Guid id)
        {
            try
            {
                var group = _parameterGroupRepository.Find(id);
                if (group != null)
                {
                    var subs = _parameterGroupRepository.Table.Where(c => c.ParentId == id);
                    _parameterGroupRepository.RemoveHardRange(subs);
                    _parameterGroupRepository.Remove(group);
                    if (group.Image == Guid.Empty)
                    {
                        Domain.File.Remove(_fileRepository, (Guid)group.Image);
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

        public IActionResult RemoveParameter(Guid id)
        {
            try
            {
                var parameter = _parameterRepository.Find(id);
                if (parameter != null)
                {
                    _parameterRepository.Remove(parameter);
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

        [HttpPost]
        public IActionResult SaveGroupTranslation(UtilitiesViewModel model)
        {
            try
            {
                var groupTranslation = _parameterGroupTranslationRepository.Find(model.ParameterGroupTranslation.Id);
                if (groupTranslation == null)
                {
                    _parameterGroupTranslationRepository.Add(model.ParameterGroupTranslation);
                }
                else
                {
                    groupTranslation.Name = model.ParameterGroupTranslation.Name;
                    groupTranslation.LanguageId = model.ParameterGroupTranslation.LanguageId;
                    _parameterGroupTranslationRepository.Update(groupTranslation);
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

        [HttpPost]
        public IActionResult SaveParameterTranslation(UtilitiesViewModel model)
        {
            try
            {
                var parameterTranslation = _parameterTranslationRepository.Find(model.ParameterTranslation.Id);
                if (parameterTranslation == null)
                {
                    _parameterTranslationRepository.Add(model.ParameterTranslation);
                }
                else
                {
                    parameterTranslation.Name = model.ParameterTranslation.Name;
                    parameterTranslation.LanguageId = model.ParameterTranslation.LanguageId;
                    _parameterTranslationRepository.Update(parameterTranslation);
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
        

        public IActionResult RemoveGroupTranslation(Guid id)
        {
            try
            {
                var groupTranslation = _parameterGroupTranslationRepository.Find(id);
                if (groupTranslation != null)
                {
                    _parameterGroupTranslationRepository.Remove(groupTranslation);
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

        public IActionResult RemoveParameterTranslation(Guid id)
        {
            try
            {
                var parameterTranslation = _parameterTranslationRepository.Find(id);
                if (parameterTranslation != null)
                {
                    _parameterTranslationRepository.Remove(parameterTranslation);
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
