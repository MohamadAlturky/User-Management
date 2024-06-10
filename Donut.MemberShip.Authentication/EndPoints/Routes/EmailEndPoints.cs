using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Donut.MemberShip.Authentication.EndPoints.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Donut.MemberShip.Core.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Donut.MemberShip.Authentication.EndPoints.Installer;

namespace Donut.MemberShip.Authentication.EndPoints.Routes;

public class EmailEndPoints : IEndPointInstaller
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {

        var routeGroup = endpoints.MapGroup(RouteSettings.EMAIL_ROUTE_GROUP);
        var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
        var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
        var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();
        var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

        // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
        string? confirmEmailEndpointName = null;

        routeGroup.MapGet("/confirmEmail", async Task<Results<ContentHttpResult, UnauthorizedHttpResult>>
              ([FromQuery] string userId, [FromQuery] string code, [FromQuery] string? changedEmail, [FromServices] IServiceProvider sp) =>
        {
           
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByIdAsync(userId) is not { } user)
            {
                // We could respond with a 404 instead of a 401 like Identity UI, but that feels like unnecessary information.
                return TypedResults.Unauthorized();
            }

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return TypedResults.Unauthorized();
            }

            IdentityResult result;

            if (string.IsNullOrEmpty(changedEmail))
            {
                result = await userManager.ConfirmEmailAsync(user, code);
            }
            else
            {
                // As with Identity UI, email and user name are one and the same. So when we update the email,
                // we need to update the user name.
                result = await userManager.ChangeEmailAsync(user, changedEmail, code);

                if (result.Succeeded)
                {
                    result = await userManager.SetUserNameAsync(user, changedEmail);
                }
            }

            if (!result.Succeeded)
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Text("Thank you for confirming your email.");
        })
          .Add(endpointBuilder =>
          {
              var finalPattern = ((RouteEndpointBuilder)endpointBuilder).RoutePattern.RawText;
              confirmEmailEndpointName = $"{nameof(Donut.MemberShip.Authentication)}-{finalPattern}";
              endpointBuilder.Metadata.Add(new EndpointNameMetadata(confirmEmailEndpointName));
          });

        routeGroup.MapPost("/resendConfirmationEmail", async Task<Ok>
            ([FromBody] ResendConfirmationEmailRequest resendRequest, HttpContext context, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user)
            {
                return TypedResults.Ok();
            }

            await SendConfirmationEmailAsync(user, userManager, context, resendRequest.Email);
            return TypedResults.Ok();
        });
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
    }
}
