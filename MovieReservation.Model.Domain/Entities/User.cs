namespace MovieReservation.Model.Domain;

public class User {

    public long Id { get; set; }
    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;

    public ICollection<Rol>? Roles { get; set; }

}