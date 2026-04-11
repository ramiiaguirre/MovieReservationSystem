namespace MovieReservation.API;

public class ApiResponse<T>
{
    public string Status { get; set; } = "success";
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static ApiResponse<T> Success(T data) =>
        new() { Status = "success", Data = data };

    public static ApiResponse<T> Fail(string message) =>
        new() { Status = "error", Data = default, Message = message };
}
