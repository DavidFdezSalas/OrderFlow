using Asp.Versioning;
using Microsoft.AspNetCore.Routing;

namespace TiendaDavid.Features.Auth
{
    public static class AuthGroup
    {
        public static RouteGroupBuilder MapAuthGroup(this IEndpointRouteBuilder routes)
        {
            var versionSet = routes.NewApiVersionSet("v1")
                .HasApiVersion(new Asp.Versioning.ApiVersion(1, 0))
                .ReportApiVersions()
                .Build();

            var group = routes.MapGroup("/api/v{version:apiVersion}/auth");

            group.WithApiVersionSet(versionSet);
            group.WithTags("Auth");
            return group;
        }
    }
}