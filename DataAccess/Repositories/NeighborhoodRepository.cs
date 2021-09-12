using Data.Models.General;

namespace DataAccess.Repositories
{
    public class NeighborhoodRepository : Repository<Neighborhood>
    {
        private readonly DataContext _context;
        public NeighborhoodRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
