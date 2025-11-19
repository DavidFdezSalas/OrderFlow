using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TiendaDavid.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
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


        public record RegisterRequest()
        {
            public required string UserName { get; set; }
            public required string Password { get; set; }
            public required string Email { get; set; }
        }

        public record RegisterResponse()
        {
            public string Message { get; set; }
            public string? Email { get; set; }
            public IEnumerable<string> Errors { get; set; }
        }
    }
}
