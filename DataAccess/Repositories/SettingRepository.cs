using Data.Models;

namespace DataAccess.Repositories
{
    public class SettingRepository : Repository<Setting>
    {
        private readonly DataContext _context;
        public SettingRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
