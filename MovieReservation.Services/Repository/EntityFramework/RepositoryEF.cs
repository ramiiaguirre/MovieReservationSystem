using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Services;

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

    public async Task Delete(long id)
    {
        T? data = await _dbSet.FindAsync(id);

        if (data is not null)
            _dbSet.Remove(data);
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbContext.Set<T>().AsQueryable();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<T?> Get(long id)
    {
        T? data = await _dbSet.FindAsync(id);
        return data;
    }

    public async Task<T?> Get(string name)
    {
        return await _dbContext.Set<T>()
            .FirstOrDefaultAsync(e => EF.Property<string>(e, "Name") == name);
    }

    public async Task<IEnumerable<T>> GetAll()
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
