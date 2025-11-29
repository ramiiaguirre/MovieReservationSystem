using MovieReservation.Model.Domain;

public class Rol
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdateAt { get; set; }

    public virtual ICollection<User>? Users { get; set; }

    public virtual IEnumerable<Permission>? Permissions { get; set; }

}