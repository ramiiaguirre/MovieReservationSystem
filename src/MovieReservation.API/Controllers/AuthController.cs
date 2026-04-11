using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO request)
    {  
        try
        {
            UserDTO userCreated = await _authService.SignUp(request);
            
            if (!string.IsNullOrEmpty(userCreated.Name))
                return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
            else 
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }
        catch(Exception e)
        {
            return StatusCode(StatusCodes.Status409Conflict, e.Message);
        }

    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> LogIn(LogInDTO request)
    {
        var user = await _authService.LogIn(request);

        if (user == null)
        {
            return StatusCode(StatusCodes.Status404NotFound,
                new { isSuccess = false, token = ""}
            );
        }
        else
        {
            return StatusCode(StatusCodes.Status200OK,
                new { isSuccess = true, token = _jwtManager.GenerateJWT(user)}
            );
        }
    }

    [HttpGet]
    [Route("validarToken")]
    public IActionResult ValidateToken([FromQuery] string token)
    {
        bool IsValidToken = _jwtManager.ValidarToken(token);
        return StatusCode(StatusCodes.Status200OK, new { isSuccess = IsValidToken });
    }

    [HttpPost]
    [Route("logout")]
    public ActionResult LogOut()
    {
        //Client delete token
        throw new NotImplementedException();

        // To do Blacklist in Redis?
    }

}