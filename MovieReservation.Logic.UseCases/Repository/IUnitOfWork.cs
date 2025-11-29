using MovieReservation.Model.Domain;

namespace MovieReservation.Logic.Repository;
public interface IUnitOfWork
{
    public IRepository<User> Users { get; }

    public Task Save();
}