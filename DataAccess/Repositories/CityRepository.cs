using Data.Models.General;

namespace DataAccess.Repositories
{
    public class CityRepository : Repository<City>
    {
        private readonly DataContext _context;
        public CityRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
