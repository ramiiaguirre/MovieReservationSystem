namespace MovieReservation.Model.Domain;

public class Role
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public virtual ICollection<User>? Users { get; set; }

    public virtual IEnumerable<Permission>? Permissions { get; set; }

}