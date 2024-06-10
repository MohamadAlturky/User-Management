using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Donut.MemberShip.Authentication.EndPoints.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Donut.MemberShip.Core.Models;
using Microsoft.Extensions.Options;
using Donut.MemberShip.Authentication.EndPoints.Installer;

namespace Donut.MemberShip.Authentication.EndPoints.Routes
{
    internal class JWTRefreshEndPoint : IEndPointInstaller
    {
        public void AddRoutes(IEndpointRouteBuilder endpoints)
        {
            var routeGroup = endpoints.MapGroup(RouteSettings.JWT_ROUTE_GROUP);
            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            routeGroup.MapPost("/refresh", async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
            ([FromBody] RefreshRequest refreshRequest, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<ApplicationUser>>();
                var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
                var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

                // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
                if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                    timeProvider.GetUtcNow() >= expiresUtc ||
                    await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not ApplicationUser user)

                {
                    return TypedResults.Challenge();
                }

                var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
            });
        }
    }
}
