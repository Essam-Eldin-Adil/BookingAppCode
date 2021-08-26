using Data;
using iQuarc.DataLocalization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Repository<Entity> : IRepository<Entity> where Entity : Data.Entity
{

    private readonly DataContext _dataContext;
    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public IQueryable<Entity> Table => _dataContext.Set<Entity>();


    public Guid Add(Entity entity)
    {
        try
        {
           
            SetValues(entity);
            _dataContext.Set<Entity>().Add(entity);
            _dataContext.SaveChanges();

            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Guid();

        }
    }



    public async Task<Guid> AddAsync(Entity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            SetValues(entity);
            await _dataContext.Set<Entity>().AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return entity.Id;
        }
        catch (Exception ex)
        {
            return new Guid();
        }
    }

    public void AddRange(IEnumerable<Entity> entities)
    {
        _dataContext.Set<Entity>().AddRange(entities);
        _dataContext.SaveChanges();
    }

    public async Task AddRangeAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default)
    {
        await _dataContext.Set<Entity>().AddRangeAsync(entities);
        _dataContext.SaveChanges();
    }

    public void Dispose()
    {
        _dataContext.Dispose();
    }

    public Entity Find(object id)
    {
        return _dataContext.Set<Entity>().Find(id);
    }

    public async Task<Entity> FindAsync(object id)
    {
        var entity = await _dataContext.Set<Entity>().FindAsync(id);
        return entity;
    }
    public IEnumerable<Entity> Get(string languageCode = null)
    {
        var model = Table;

        if(languageCode != null && languageCode != string.Empty)
        {
            return model.Localize(new CultureInfo(languageCode)).AsEnumerable<Entity>();
        }
        return model.Where(w => w.IsDeleted == false).AsEnumerable<Entity>();
    }

    public IEnumerable<Entity> Get(Expression<Func<Entity, bool>> filter, string languageCode = null)
    {
        var model = Table.Where(filter);
        if (!string.IsNullOrEmpty(languageCode))
        {
            return model.Localize(new CultureInfo(languageCode)).AsEnumerable<Entity>();
        }
        return model.AsEnumerable<Entity>();
    }

    public string GetEntityName()
    {
        return "Entity Name: " + typeof(Entity).Name;
    }

    //public void Remove(Entity entity)
    //{
    //    entity.IsDeleted = true;
    //    _dataContext.Set<Entity>().Update(entity);
    //     _dataContext.SaveChanges();
    //}
    //public async Task RemoveAsync(Entity entity)
    //{
    //    entity.IsDeleted = true;
    //    _dataContext.Set<Entity>().Update(entity);
    //    await _dataContext.SaveChangesAsync();
    //}
    public void Restore(object id)
    {
        try
        {
            var entity = Find(id);
            if (entity != null)
            {
                entity.IsDeleted = false;
                _dataContext.Entry(entity).State = EntityState.Modified;
                _dataContext.SaveChanges();
            }
        }
        catch
        {

        }

    }
    public async Task RestoreAsync(object id)
    {
        try
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = false;
                _dataContext.Entry(entity).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
            }
        }
        catch
        {

        }
    }



    public bool RemoveSoft(object id)
    {
        try
        {
            var entity = Find(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _dataContext.Set<Entity>().Update(entity);
                _dataContext.SaveChanges();
                return true;
            }
        }
        catch
        {
            return false;

        }

        return false;
    
    }
    public async Task<bool> RemoveSoftAsync(object id)
    {
        try
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                await UpdateAsync(entity);
                return true;
            }
        }
        catch
        {
            return false;

        }

        return false;

    }
    public bool RemoveHard(object id)
    {
        try
        {
            var entity = Find(id);
            if (entity != null)
            {
                _dataContext.Set<Entity>().Remove(entity);
                _dataContext.SaveChanges();
            }
        }
        catch
        {
            return false;

        }

        return false;
       
    }
    public bool Remove(Entity entity)
    {
        try
        {
            _dataContext.Set<Entity>().Remove(entity);
            _dataContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;

        }
    }
    public async Task<bool> RemoveHardAsync(object id)
    {
        try
        {
            var entity = await FindAsync(id);
            if (entity != null)
            {
                 _dataContext.Set<Entity>().Remove(entity);
               await _dataContext.SaveChangesAsync();
                return true;
            }
        }
        catch
        {
            return false;

        }

        return false;

    }
    public async Task<bool> RemoveSoftRange(IEnumerable<Entity> entities)
    {
        try
        {

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                await UpdateAsync(entity);

            }
            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> RemoveHardRange(IEnumerable<Entity> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                _dataContext.Set<Entity>().Remove(entity);
                
            }
            await _dataContext.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }

    }
    public async Task<List<Entity>> ToListAsync()
    {
       return await Table.ToListAsync();

    }
    //public Entity Old(object id)
    //{
    //   return Find(id);
    //}

    public void Update(Entity entity)
    {

        _dataContext.Attach(entity);
        _dataContext.Entry(entity).State = EntityState.Modified;

        //_dataContext.Set<Entity>().Update(entity);
        _dataContext.SaveChanges();
    }
    public async Task UpdateAsync(Entity entity)
    {
        _dataContext.Attach(entity);
        _dataContext.Entry(entity).State = EntityState.Modified;
        //_dataContext.Set<Entity>().Update(entity);
        await _dataContext.SaveChangesAsync();
    }

    public void UpdateRange(IEnumerable<Entity> entities)
    {
        foreach(var entity in entities)
        {
            _dataContext.Attach(entity);
            var entry = _dataContext.Entry(entity);
            entry.State = EntityState.Modified;
        }
        _dataContext.SaveChanges();

        //if (entities.Count() > 0)
        //{
        //    _dataContext.Set<Entity>().UpdateRange(entities);
        //    _dataContext.SaveChanges();

        //}
    }

    public  bool Any(Expression<Func<Entity, bool>> filter)
    {
        return  _dataContext.Set<Entity>().Any(filter);
    }

    public async Task<bool> AnyAsync(Expression<Func<Entity, bool>> filter)
    {
        return await _dataContext.Set<Entity>().AnyAsync(filter);
    }

    public bool IsDeleted(object id)
    {
        if(id == null)
        {
            return false;
        }
        Guid Id = (Guid)id;
        return  _dataContext.Set<Entity>().Any(a => a.Id == Id && a.IsDeleted == true);
    }

    public void SetValues(Entity entity)
    {
        entity.Id = Guid.NewGuid();
        entity.IsDeleted = false;
        entity.CreatedDate = DateTime.Now;
    }
}