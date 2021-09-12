using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class ChaletParameterRepository : Repository<ChaletParameterValue>
    {
        private readonly DataContext _context;
        public ChaletParameterRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}