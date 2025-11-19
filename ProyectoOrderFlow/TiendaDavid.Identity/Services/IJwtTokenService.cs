using Microsoft.AspNetCore.Identity;

namespace TiendaDavid.Identity.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
