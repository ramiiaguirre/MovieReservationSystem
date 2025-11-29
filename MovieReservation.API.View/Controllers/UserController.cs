using MovieReservation.Logic.Repository;
using MovieReservation.Model.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieReservation.API.View.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGetUserById _getUserById;
    private readonly IGetUsers _getUsers;
    private readonly ICreateUser _createUser;

    public UserController(IGetUserById getUserById, IGetUsers getUsers, ICreateUser createUser)
    {
        _getUserById = getUserById;
        _getUsers = getUsers;
        _createUser = createUser;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet(Name = "GetUsers")]
    public async Task<IEnumerable<UserDTO>> Get()
    {
        var users = await _getUsers.Execute();
        return users.Select(u => new UserDTO { Name = u.Name }).ToArray();
    }

    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<ActionResult<UserDTO>> Get(long id)
    {
        var user = await _getUserById.Execute(id, true);

        if (user is null)
        {
            return NotFound(new
            {
                message = "Usuario no encontrado"
            });
        }

        var roles = new List<string>();
        foreach (var rol in user.Roles ?? Enumerable.Empty<Rol>())
        {
            Console.WriteLine(rol.Name);
            roles.Add(rol.Name);
        }    
        
        return Ok(new UserDTO
        {
            Name = user.Name,
            Roles = roles.ToList()
        });
    }
    
}
