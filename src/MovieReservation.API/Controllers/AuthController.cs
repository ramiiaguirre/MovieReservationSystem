using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.API;
using MovieReservation.Services;

namespace MovieReservation.API;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtManager _jwtManager;
    private  readonly IAuthService _authService;
    public AuthController(JwtManager jwtManager, IAuthService authService)
    {
        _jwtManager = jwtManager;
        _authService = authService;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("signup")]
    public async Task<Results<
        Created, 
        Conflict<ProblemDetails>>>  
        SignUp([FromBody] SignUpDTO request)
    {  
        try
        {
            UserDTO userCreated = await _authService.SignUp(request);
            return TypedResults.Created();
        }
        catch(Exception e)
        {
            return TypedResults.Conflict(new ProblemDetails()
            {
                Title = "Movie Not Found",
                Detail = $"{e.Message}",
                Status = StatusCodes.Status409Conflict,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
            });
        }

    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<Results<
        NotFound<ProblemDetails>, Ok>>
         LogIn(LogInDTO request)
    {
        var user = await _authService.LogIn(request);

        if (user == null)
        {
            return TypedResults.NotFound(new ProblemDetails()
            {
                Title = "User Not Found",
                Detail = $"User Not Found with the provided data",
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            });
        }
        else
        {
            Response.Cookies.Append("jwt", _jwtManager.GenerateJWT(user), new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
            });
            return TypedResults.Ok();
        }
    }

    [HttpOptions]
    [Route("validarToken")]
    public IActionResult ValidateToken([FromQuery] string token)
    {
        bool IsValidToken = _jwtManager.ValidarToken(token);
        return StatusCode(StatusCodes.Status200OK, IsValidToken);
    }

    // [HttpPost]
    // [Route("logout")]
    // public ActionResult LogOut()
    // {
    //     //Client delete token
    //     throw new NotImplementedException();

    //     // To do Blacklist in Redis?
    // }

}