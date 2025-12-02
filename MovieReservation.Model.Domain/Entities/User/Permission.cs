namespace MovieReservation.Model.Domain;

public class Permission
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<Role>? Roles { get; set; }
}