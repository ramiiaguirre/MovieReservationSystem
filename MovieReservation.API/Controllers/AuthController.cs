using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.API;
using MovieReservation.Services;

namespace MyApp.Namespace
{
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
            UserDTO userCreated = await _authService.SignUp(request);

            if (!string.IsNullOrEmpty(userCreated.Name))
                return StatusCode(StatusCodes.Status201Created, new { isSuccess = true });
            else 
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> LogIn(LogInDTO request)
        {
            return null;
        }

        [HttpGet]
        [Route("validarToken")]
        public IActionResult ValidateToken([FromQuery] string token)
        {
            bool tokenValido = _jwtManager.ValidarToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = tokenValido });
        }

        [HttpPost]
        [Route("logout")]
        public ActionResult LogOut()
        {
            return null;
        }
    }
}
