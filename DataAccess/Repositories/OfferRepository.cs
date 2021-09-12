using Data.Models.Chalets.ChaletDetails;

namespace DataAccess.Repositories
{
    public class OfferRepository : Repository<Offer>
    {
        private readonly DataContext _context;
        public OfferRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}