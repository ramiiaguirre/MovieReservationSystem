using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MovieReservation.Services;

public class RepositoryEF<T> : IRepository<T> where T : class 
{
    private readonly MovieReservationContext _dbContext;
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
        // await _dbSet.Where(e => EF.Property<long>(e, "Id") == id).ExecuteDeleteAsync();
        T? data = await _dbSet.FindAsync(id);

        if (data is not null)
            _dbSet.Remove(data);
    }

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        var query = _dbSet.AsQueryable();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.Where(predicate).ToListAsync();
    }

    public async Task<T?> Get(long id, params Expression<Func<T, object>>[] includes)
    {
        if (includes.Length == 0)
            return await _dbSet.FindAsync(id);   // aprovecha el caché del ChangeTracker

        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
    }

    public async Task<T?> FindOne(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        if (includes.Length > 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(predicate);
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
        // await Task.Run(() =>
        // {
        //     _dbSet.Attach(data);
        //     _dbContext.Entry(data).State = EntityState.Modified;            
        // });
        _dbSet.Update(data); // Attach + State

        // return Task.FromResult(data);
        return data;
    }
}
