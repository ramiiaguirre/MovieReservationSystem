namespace MovieReservation.Domain;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;

    public IEnumerable<Rol>? Roles { get; set; }
}
