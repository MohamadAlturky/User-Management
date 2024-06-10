using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Donut.MemberShip.Core.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Donut.MemberShip.Authentication.EndPoints.Utilities;
using Donut.MemberShip.Authentication.EndPoints.Settings;
using Donut.MemberShip.Authentication.EndPoints.Installer;

namespace Donut.MemberShip.Authentication.EndPoints.Routes;

public class RegisterEndPoint : IEndPointInstaller
{
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();

    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {

        var routeGroup = endpoints.MapGroup(RouteSettings.API_ROUTE_GROUP);

        routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] RegisterRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
        {
            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
            string? confirmEmailEndpointName = null;

            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException($"{nameof(Authentication)} requires a user store with email support.");
            }

            var userStore = sp.GetRequiredService<IUserStore<ApplicationUser>>();
            var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            var email = registration.Email;

            if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
            {
                return EndPointUtility.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
            }

            var user = new ApplicationUser();
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, registration.Password);

            if (!result.Succeeded)
            {
                return EndPointUtility.CreateValidationProblem(result);
            }

            await SendConfirmationEmailAsync(user, userManager, context, email);
            async Task SendConfirmationEmailAsync(ApplicationUser user, UserManager<ApplicationUser> userManager, HttpContext context, string email, bool isChange = false)
            {
                if (confirmEmailEndpointName is null)
                {
                    throw new NotSupportedException("No email confirmation endpoint was registered!");
                }

                var code = isChange
                    ? await userManager.GenerateChangeEmailTokenAsync(user, email)
                    : await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var userId = await userManager.GetUserIdAsync(user);
                var routeValues = new RouteValueDictionary()
                {
                    ["userId"] = userId,
                    ["code"] = code,
                };

                if (isChange)
                {
                    // This is validated by the /confirmEmail endpoint on change.
                    routeValues.Add("changedEmail", email);
                }

                var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
                    ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

                await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
            }
            return TypedResults.Ok();
        });
    }
}
