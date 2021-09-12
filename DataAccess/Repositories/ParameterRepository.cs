using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class ParameterRepository : Repository<Parameter>
    {
        private readonly DataContext _context;
        public ParameterRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}