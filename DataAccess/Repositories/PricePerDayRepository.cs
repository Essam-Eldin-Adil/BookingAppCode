using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class PricePerDayRepository : Repository<PricePerDay>
    {
        private readonly DataContext _context;
        public PricePerDayRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}