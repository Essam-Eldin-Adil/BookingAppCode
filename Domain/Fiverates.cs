using Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Fiverates
    {
        //private static readonly;
        public static bool IsFive(HttpContext httpContext, Guid id)
        {
            var user = SessionClass.GetUser(httpContext);
            var fiverateRepository = (IRepository<Data.Models.Fiverate>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Fiverate>));
            return fiverateRepository.Table.Any(c=>c.UserId==user.Id&&c.UnitId==id);
        }
        public static bool AddRemoveFive(HttpContext httpContext, Guid id)
        {
            var user = SessionClass.GetUser(httpContext);
            var fiverateRepository = (IRepository<Data.Models.Fiverate>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Fiverate>));
            var five = fiverateRepository.Table.FirstOrDefault(c => c.UnitId == id && c.UserId == user.Id);
            if (five==null)
            {
                five= new Fiverate();
                five.UserId = user.Id;
                five.UnitId = id;
                fiverateRepository.Add(five);
                return true;
            }
            else
            {
                fiverateRepository.Remove(five);
                return false;
            }
        }
    }
}
