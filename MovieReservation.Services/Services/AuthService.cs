using MovieReservation.API;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _repository = default!;
    private readonly IPasswordHasher _passwordHasher = default!;

    public AuthService(IRepository<User> repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }


    public async Task<UserDTO> SignUp(SignUpDTO request)
    {
        // First To do Validations

        var userCreated = await _repository.Add(new User()
        {
            Name = request.Name,
            Password = _passwordHasher.HashPassword(request.Password)
        });

        await _repository.Save();

        return new UserDTO()
        {
            Name = userCreated.Name
        };
    }
    public Task<UserDTO> LogIn(LogInDTO request)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> LogOut(SignUpDTO request)
    {
        throw new NotImplementedException();
    }

}
