using Data.Models.Chalets;

namespace DataAccess.Repositories
{
    public class ChaletImageRepository : Repository<ChaletImage>
    {
        private readonly DataContext _context;
        public ChaletImageRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
