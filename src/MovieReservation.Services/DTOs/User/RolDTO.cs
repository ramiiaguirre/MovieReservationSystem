namespace MovieReservation.Services;

public record RolDTO
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}


public record RolToUserRequest
{
    public long UserId { get; set; }
    public string RolName { get; set; } = default!;
}