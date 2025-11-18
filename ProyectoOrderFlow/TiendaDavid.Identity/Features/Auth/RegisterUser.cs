using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace TiendaDavid.Features.Auth
{
    public static class RegisterUser
    {
        public record Request(string nombre, string email, string password);
        public record Response(string userId, string nombre, string email);

        public static IEndpointRouteBuilder MapRegisterUser(this IEndpointRouteBuilder group)
        {
            var Authgroup = group.MapAuthGroup();
            Authgroup.MapPost("/register", HandleAsync)
                .WithName("register")
                .AllowAnonymous();

            return group;
        }

        private static async Task<IResult> HandleAsync(Request request, UserManager<IdentityUser> userManager)
        {

            var user = new IdentityUser
            {
                UserName = request.nombre,
                Email = request.email
            };
            var result = await userManager.CreateAsync(user, request.password);

            return Results.Ok();
        }
    }
}
