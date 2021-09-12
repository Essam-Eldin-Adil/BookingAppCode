using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class UnitRepository : Repository<Unit>
    {
        private readonly DataContext _context;
        public UnitRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}