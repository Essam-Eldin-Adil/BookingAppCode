using Data.Models.Chalets;

namespace DataAccess.Repositories
{
    public class RegionRepository : Repository<Region>
    {
        private readonly DataContext _context;
        public RegionRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}