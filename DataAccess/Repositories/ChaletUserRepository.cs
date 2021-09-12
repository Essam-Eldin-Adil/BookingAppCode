using Data.Models.Chalets;

namespace DataAccess.Repositories
{
    public class ChaletUserRepository : Repository<ChaletUser>
    {
        private readonly DataContext _context;
        public ChaletUserRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
