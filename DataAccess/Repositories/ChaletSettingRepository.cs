using Data.Models;
using Data.Models.Chalets;

namespace DataAccess.Repositories
{
    public class ChaletSettingRepository : Repository<ChaletSetting>
    {
        private readonly DataContext _context;
        public ChaletSettingRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
