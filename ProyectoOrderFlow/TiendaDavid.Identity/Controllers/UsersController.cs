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

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {id}", id);
                return NotFound(new { message = "User not found", id = id });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Error deleting user: {Errors}", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = "Error deleting user:",errors = result.Errors.Select(e => e.Description)});
            }

            _logger.LogInformation("User deleted: {name}", user.UserName);
            return NoContent();

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

        public record UserUpdateResponse
        {
            public required string UserName { get; init; }
            public required string Message { get; init; }
            public IEnumerable<string>? Errors { get; set; }

        }

        public record UserUpdateRequest
        {
            public required string UserName { get; init; }
            public required string Password { get; init; }
        }
    }
}
