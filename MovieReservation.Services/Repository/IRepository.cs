namespace MovieReservation.Services;

public interface IRepository<T> where T : class
{
    Task<T?> Get(long id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T data);
    Task<T> Update(T data);
    Task Delete(long id);
    Task Save();
}
