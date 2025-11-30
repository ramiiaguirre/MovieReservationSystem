namespace MovieReservation.Model.Domain;

public class User {

    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;

    public DateTime? CreatedAt { get; set; } = default!;
    public DateTime? UpdatedAt { get; set; } = default!;

    public ICollection<Role>? Roles { get; set; }

}