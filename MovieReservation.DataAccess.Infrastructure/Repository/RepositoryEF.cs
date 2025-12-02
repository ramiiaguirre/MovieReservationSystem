using System.Linq.Expressions;
using MovieReservation.Logic.UseCases;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.DataAccess.Infrastructure;

public class RepositoryEF<T> : IRepository<T> where T : class 
{
    private MovieReservationContext _dbContext;
    private DbSet<T> _dbSet;

    public RepositoryEF(MovieReservationContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> Add(T data)
    {
        await _dbSet.AddAsync(data);
        return data;
    }

    public async Task<bool> Delete(long id)
    {
        T? data = await _dbSet.FindAsync(id);

        if (data is not null)
        {
            _dbSet.Remove(data);
            return true;
        }
        return false;
    }

    public async Task<T?> Get(long id)
    {
        T? data = await _dbSet.FindAsync(id);
        return data;
    }

    public async Task<T?> Get(long id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;
        
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        
        return await query.FirstOrDefaultAsync(predicate);
    }


    public async Task<IEnumerable<T>> Get()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<T> Update(T data)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(data);
            _dbContext.Entry(data).State = EntityState.Modified;            
        });
        return data;
    }
}