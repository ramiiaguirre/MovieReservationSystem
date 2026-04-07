using MovieReservation.Services;

namespace MovieReservation.API;

public interface IAuthService
{
    public Task<UserDTO> SignUp(SignUpDTO request);
    public Task<UserDTO?> LogIn(LogInDTO request);
    public Task<UserDTO> LogOut();
}
