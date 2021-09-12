using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class ParameterGroupRepository : Repository<ParameterGroup>
    {
        private readonly DataContext _context;
        public ParameterGroupRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}