using Data.Models;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly DataContext _context;
        public UserRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
