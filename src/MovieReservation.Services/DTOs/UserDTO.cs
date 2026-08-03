using MovieReservation.Domain;

namespace MovieReservation.Services;

public class UserDTO
{
    public long Id { get; set; }

    public string Name { get; set; } = default!;

    public List<Rol> Roles { get; set; } = new List<Rol>();
}
