using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly JwtManager _jwtManager;
    private  readonly UserService _userService;
    public UserController(JwtManager jwtManager, UserService service)
    {
        _jwtManager = jwtManager;
        _userService = service;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<Results<
        Created<RolDTO>, 
        Conflict<ProblemDetails>>>  
        CreateRol([FromBody] RolDTO request)
    {  
        try
        {
            RolDTO? rolCreated = await _userService.CreateRol(request);
            return TypedResults.Created($"/User/{rolCreated.Name}", rolCreated);
        }
        catch(Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Rol Not Found",
                Detail = $"{e.Message}",
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            });
        }

    }

    [HttpGet, Route("rol")]
    [AllowAnonymous]
    public async Task<Results<
        Ok<List<RolDTO>>,
        BadRequest>>
        CreateRol()
    {
        var roles = await _userService.GetRoles();
        return TypedResults.Ok(roles);
    }

    [HttpPost, Route("addRolToUser")]
    public async Task<Results<Ok<bool>, NotFound<ProblemDetails>>> AddRolToUser([FromBody] RolToUserRequest request)
    {
        try
        {
            bool result = await _userService.AddRolToUser(request);
            return TypedResults.Ok(result);    
        }
        catch (KeyNotFoundException e)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "Not Found",
                Detail = $"{e.Message}"
            }); 
        }
    }
        

}