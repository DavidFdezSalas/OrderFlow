using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TiendaDavid.Identity.Services;

namespace TiendaDavid.Identity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, IConfiguration configuration, IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Error creating user: {Errors}", result.Errors.Select(e => e.Description));

                return BadRequest(new RegisterResponse()
                {
                    Email = request.Email,
                    Message = "Error creating user",
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new RegisterResponse()
            {
                Email = request.Email,
                Message = "User created successfully"
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogWarning("Invalid email or password");
                return Unauthorized(new LoginResponse()
                {
                    Message = "Invalid email or password",
                    Token = null,
                    Email = null
                });
            }

            var token = _jwtTokenService.GenerateToken(user);
            _logger.LogInformation("Login succesful");
            return Ok(new LoginResponse()
            {
                Message = "Login succesful",
                Token = token,
                Email = request.Email
            });
        }


        public record RegisterRequest()
        {
            public required string UserName { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }

        }

        public record RegisterResponse()
        {
            public string Message { get; set; } = string.Empty;
            public string? Email { get; set; }
            public IEnumerable<string>? Errors { get; set; }
        }

        public record LoginRequest()
        {
            public required string Email { get; set; }
            public required string Password { get; set; }
        }

        public record LoginResponse()
        {
            public string Message { get; set; }
            public string? Token { get; set; }
            public string? Email { get; set; }
        }
    }
}
