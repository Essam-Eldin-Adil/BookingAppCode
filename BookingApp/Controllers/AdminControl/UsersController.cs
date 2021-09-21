using Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Resources;
using static System.Int32;

namespace BookingApp.Controllers.AdminControl
{
    public class UsersController : BaseController
    {
        private readonly IRepository<User> _userRepository;

        public UsersController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(new User());
        }

        public ActionResult<IEnumerable<User>> GetData()
        {
            try
            {
                var query = Request.Query;
                int take = 0;
                int skip = 0;
                int userType = 0;
                TryParse(query["length"], out take);
                TryParse(query["start"], out skip);
                TryParse(query["userType"], out userType);
                var search = query["search[value]"][0];
                var draw = query["draw"];
                var data = _userRepository.Table.Where(c=> c.UserType==userType&&(string.IsNullOrEmpty(search) || c.Email==search
                                                                                         ||c.FirstName==search||
                                                            c.LastName==search||c.WhatsAppNumber==search
                                                                                         ||c.PhoneNumber==search)).OrderBy(a => a.CreatedDate).Skip(skip).Take(take).ToList(); ;
                return Ok(new { draw = draw, data, recordsTotal = _userRepository.Table.Count(c=>c.UserType== userType), recordsFiltered = _userRepository.Table.Count(c=>c.UserType== userType) });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult SaveUser(User model)
        {
            try
            {
                var user = _userRepository.Find(model.Id);
                if (user==null)
                {
                    user=new User();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.WhatsAppNumber = model.WhatsAppNumber;
                    user.Email = model.Email;
                    user.BirthDate = model.BirthDate;
                    user.Status = model.Status;
                    user.UserType = model.UserType;
                    _userRepository.Add(user);
                }
                else
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.WhatsAppNumber = model.WhatsAppNumber;
                    user.Email = model.Email;
                    user.BirthDate = model.BirthDate;
                    user.Status = model.Status;
                    user.UserType = model.UserType;
                    _userRepository.Update(user);
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

        public IActionResult RemoveUser(Guid id)
        {
            try
            {
                var user = _userRepository.Find(id);
                if (user!=null)
                {
                    _userRepository.Remove(user);
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
