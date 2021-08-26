using DataAccess.Repositories;
using Repositories;

public class UnitOfWork : IUnitOfWork
{

    public DataContext _context { get; set; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Files = new FileRepository(_context);
    }
    public FileRepository Files { get; set; }
    

    public void Save()
    {
        _context.SaveChanges();
    }


    public void Dispose()
    {
        _context.Dispose();
    }

}