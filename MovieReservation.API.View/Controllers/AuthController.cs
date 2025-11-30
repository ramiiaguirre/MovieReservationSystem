using MovieReservation.API.View.Helpers;
using MovieReservation.Model.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieReservation.API.View.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{  
    private readonly ISignUp _signUp;
    private readonly ILogIn _logIn;
    private readonly UtilsJWT _utilsJWT;

    public AuthController(ISignUp signUp, ILogIn logIn, UtilsJWT utilsJWT)
    {
        _signUp = signUp;
        _logIn = logIn;
        _utilsJWT = utilsJWT;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("signup")]
    public async Task<IActionResult> SignUp(LoginDTO request)
    {
        var userCreated = await _signUp.Execute(
            new User
            {
                Name = request.Name,
                Password = _utilsJWT.EncryptSHA256(request.Password)
            }
        );
        
        if (userCreated.Id != 0)
            return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
        else 
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<IActionResult> Login(LoginDTO request)
    {
        var loggedInUser = await _logIn
            .Execute(request.Name, _utilsJWT.EncryptSHA256(request.Password!));

        if(loggedInUser == null)
        {            
            return StatusCode(
                StatusCodes.Status404NotFound, 
                new { isSuccess = false, token = "" }
            );
        }
        else
        {
            return StatusCode(
                StatusCodes.Status200OK, 
                new { isSuccess = true, token = _utilsJWT.GenerateJWT(loggedInUser)}
            );
        }
    }

    [HttpGet]
    [Route("validarToken")]
    public IActionResult ValidateToken([FromQuery] string token)
    {
        bool tokenValido = _utilsJWT.ValidarToken(token);
        return StatusCode(StatusCodes.Status200OK, new { isSuccess = tokenValido });
    }

    [HttpPost]
    [Route("logout")]
    public ActionResult Logout()
    {
        //Get token of the authorization header
        var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        if (string.IsNullOrEmpty(token))
            return StatusCode(StatusCodes.Status400BadRequest, new { isSuccess = false, message = "Token no proporcionado" });
        
        // Add token in blacklist
        _utilsJWT.InvalidateToken(token);
        
        return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Sesión cerrada correctamente" });
    }

}