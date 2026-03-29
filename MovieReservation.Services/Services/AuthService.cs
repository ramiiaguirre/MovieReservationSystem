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

        // var users = await _repository.Find(new Dictionary<string, object>()
        // {
        //     ["Name"] = request.Name
        // });

        var user = _repository.Get(request.Name);

        if (user.Result is not null)
            throw new Exception($"User called {request.Name} already exist.");

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
    public async Task<UserDTO> LogIn(LogInDTO request)
    {
        var users = await _repository.Find(new Dictionary<string, object>()
        {
            ["Name"] = request.Name
        });

        if (users is not null)
        {
            return new UserDTO()
            {
                Name = users.First().Name
                // Roles = users.First().Roles!.Select(r => r.Name).ToList()
            };
        }
        else
            return new UserDTO();
    }

    public Task<UserDTO> LogOut()
    {
        throw new NotImplementedException();
    }

}
