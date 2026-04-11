namespace MovieReservation.Services;

public record LogInDTO
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
}
