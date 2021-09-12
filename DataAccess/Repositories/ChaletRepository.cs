using Data.Models;
using Data.Models.Chalets;

namespace DataAccess.Repositories
{
    public class ChaletRepository : Repository<Chalet>
    {
        private readonly DataContext _context;
        public ChaletRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
