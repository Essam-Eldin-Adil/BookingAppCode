using Data.Models.General;

namespace DataAccess.Repositories
{
    public class BankRepository : Repository<Bank>
    {
        private readonly DataContext _context;
        public BankRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}