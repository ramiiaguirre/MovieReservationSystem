using System.Linq.Expressions;

namespace MovieReservation.Services;

public interface IRepository<T> where T : class
{
    Task<T?> Get(long id);
    Task<T?> Get(string name);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T data);
    Task<T> Update(T data);
    Task Delete(long id);
    Task Save();
}
