using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TiendaDavid.Identity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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

            return Ok(new UserCreationResponse()
            {
                Email = request.Email,
                Message = "User created successfully"
            });


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
                return BadRequest(new { message = "Error deleting user:", errors = result.Errors.Select(e => e.Description) });
            }

            _logger.LogInformation("User deleted: {name}", user.Id);
            return NoContent();

        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<UserUpdateResponse>> Updateuser(UserUpdateRequest request, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {id}", id);
                return NotFound(new { message = "User not found", id = id });
            }

            if (request.UserName != null && request.UserName != "")
            {
                user.UserName = request.UserName;
            }

            if (request.Email != null && request.Email != "")
            {
                user.Email = request.Email;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Error updating user: {Errors}", result.Errors.Select(e => e.Description));
                return BadRequest(new { message = "Error updating user:", errors = result.Errors.Select(e => e.Description) });
            }

            _logger.LogInformation("User updated: {name}", user.Id);
            return Ok(new UserUpdateResponse()
            {
                Email = user.Email,
                UserName = user.UserName,
                Message = "User updated successfully"
            });


        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found: {id}", id);
                return NotFound(new { message = "User not found", id = id });
            }

            var response = new UserResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };

            _logger.LogInformation("User found: {id}", user.Id);
            return Ok(response);

        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var usersQuery = _userManager.Users;

            var responseQuery = usersQuery.Select(user => new UserResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });

            var userResponse = await responseQuery.ToListAsync();

            if (!userResponse.Any())
            {
                _logger.LogInformation("Empty list");
                return NoContent();
            }

            _logger.LogInformation($"{userResponse.Count} users");
            return Ok(userResponse);
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
            public required string Email { get; init; }
            public required string Message { get; init; }
            public IEnumerable<string>? Errors { get; set; }

        }

        public record UserUpdateRequest
        {
            public string UserName { get; init; }
            public string Email { get; init; }
        }

        public record UserResponse
        {
            public required string Id { get; init; }
            public required string UserName { get; init; }
            public required string Email { get; init; }

        }
    }
}