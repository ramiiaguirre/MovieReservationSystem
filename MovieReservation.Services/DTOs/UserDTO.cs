namespace MovieReservation.Services;

public class UserDTO
{
    public string Name { get; set; } = default!;

    public List<string> Roles { get; set; } = new List<string>();
}
