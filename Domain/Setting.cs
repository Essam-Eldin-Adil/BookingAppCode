
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Domain
{
    public static class Setting
    {
        public static string Get(HttpContext httpContext, string Key)
        {
            var settingRepository = (IRepository<Data.Models.Setting>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.Setting>));
            var setting = settingRepository.Table.FirstOrDefault(c => c.Key.Equals(Key));
            if (setting==null)
            {
                return "N/A";
            }
            return setting.Value;
        }
    }
}
