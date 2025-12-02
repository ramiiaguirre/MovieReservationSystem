using MovieReservation.Logic.UseCases;
using MovieReservation.Model.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MovieReservation.API.View.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGetUserById _getUserById;
    private readonly IGetUsers _getUsers;
    private readonly IRoleUseCases _roleUseCases;

    public UserController(IGetUserById getUserById, IGetUsers getUsers, IRoleUseCases roleUseCases)
    {
        _getUserById = getUserById;
        _getUsers = getUsers;
        _roleUseCases = roleUseCases;
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
        foreach (var rol in user.Roles ?? Enumerable.Empty<Role>())
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

    [HttpPost]
    [Route("createRole")]
    public async Task<ActionResult> CreateRole(RoleDTO request)
    {
        var roleCreated = await _roleUseCases.ExecuteAddRole(
            new Role
            {
                Name = request.Name,
                Description = request.Description
            }
        );

        if (roleCreated.Id != 0)
        {
            // return Created("url/role/id", roleCreated);
            return StatusCode(201, roleCreated);
        }
        else
        {
            return Ok(new
            {
                message = "Role not created"
            });
        }
    }
    

    [HttpPost]  
    [Route("addRoleToUser")]
    [Authorize]
    public async Task<ActionResult> AddRoleToUser(UserRoleDTO request)
    {
        try
        {
            var userIdClaim = base.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            long idUserWhoAssigns = Convert.ToInt64(userIdClaim);

            await _roleUseCases.ExecuteAddRoleToUser(request.IdUser, request.IdRole, idUserWhoAssigns);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }

}
