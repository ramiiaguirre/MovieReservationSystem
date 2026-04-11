namespace MovieReservation.Services;

public record SignUpDTO
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
}
