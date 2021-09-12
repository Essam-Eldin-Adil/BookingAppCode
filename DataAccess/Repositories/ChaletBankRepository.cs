using Data.Models.General;

namespace DataAccess.Repositories
{
    public class ChaletBankRepository : Repository<ChaletBank>
    {
        private readonly DataContext _context;
        public ChaletBankRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}