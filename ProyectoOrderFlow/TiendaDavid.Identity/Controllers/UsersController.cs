using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TiendaDavid.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(ILogger<UsersController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserCreationResponse>> CreateUser(UserCreationRequest request)
        {

            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                _logger.LogError("Error creating user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));

                return new UserCreationResponse()
                {
                    Email = request.Email,
                    Message = "Error creating user",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            _logger.LogInformation("User created successfully: {Email}", request.Email);

            return new UserCreationResponse()
            {
                Email = request.Email,
                Message = "User created successfully"
            };


        }


    }

    public record UserCreationResponse
    {
        public required string Email { get; set; }
        public required string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

    public record UserCreationRequest
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
