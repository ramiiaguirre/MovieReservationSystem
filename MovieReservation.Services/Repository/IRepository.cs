namespace MovieReservation.Services;

public interface IRepository<T> where T : class
{
    Task<T?> Get(long id);
    Task<T?> Get(string name);
    Task<IEnumerable<T>> Find(Dictionary<string, object> filters);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T data);
    Task<T> Update(T data);
    Task Delete(long id);
    Task Save();
}
