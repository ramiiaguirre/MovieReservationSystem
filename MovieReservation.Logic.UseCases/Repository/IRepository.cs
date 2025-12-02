using System.Linq.Expressions;

namespace MovieReservation.Logic.UseCases;

public interface IRepository<T>
{
    Task<T?> Get(long id);
    Task<T?> Get(long id, params Expression<Func<T, object>>[] includes);

    Task<T?> Get(Expression<Func<T, bool>> predicate);
    Task<T?> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<T>> Get();
    Task<T> Add(T data);
    Task<bool> Delete(long id);
    Task<T> Update(T data);

    // Task Save();
}