using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class UnitImageRepository : Repository<UnitImage>
    {
        private readonly DataContext _context;
        public UnitImageRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}