using System;
using System.Collections.Generic;
using System.Text;


public interface IUnitOfWork : IDisposable
{
    public DataContext _context { get; set; }
    void Save();

}