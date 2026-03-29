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

    public async Task Delete(int id)
    {
        T? data = await _dbSet.FindAsync(id);

        if (data is not null)
            _dbSet.Remove(data);
    }

    public Task Delete(long id)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> Get(long id)
    {
        T? data = await _dbSet.FindAsync(id);
        return data;
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
