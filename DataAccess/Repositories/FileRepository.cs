using Data.Models;

namespace DataAccess.Repositories
{
    public class FileRepository : Repository<File>
    {
        private readonly DataContext _context;
        public FileRepository(DataContext dataContext) : base(dataContext)
        {
            _context = dataContext;
        }
    }
}
