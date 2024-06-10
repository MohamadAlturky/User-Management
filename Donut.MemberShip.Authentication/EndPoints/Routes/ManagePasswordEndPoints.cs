using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text;
using Donut.MemberShip.Authentication.EndPoints.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Donut.MemberShip.Core.Models;
using Donut.MemberShip.Authentication.EndPoints.Installer;

namespace Donut.MemberShip.Authentication.EndPoints.Routes;

internal class ManagePasswordEndPoints : IEndPointInstaller
{
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        var routeGroup = endpoints.MapGroup(RouteSettings.PASSWORD_ROUTE_GROUP);
        var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<ApplicationUser>>();

        routeGroup.MapPost("/forgotPassword", async Task<Results<Ok, ValidationProblem>>
                   ([FromBody] ForgotPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(resetRequest.Email);

            if (user is not null && await userManager.IsEmailConfirmedAsync(user))
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                await emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
            }

            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return TypedResults.Ok();
        });

        routeGroup.MapPost("/resetPassword", async Task<Results<Ok, ValidationProblem>>
            ([FromBody] ResetPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
        {
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByEmailAsync(resetRequest.Email);

            if (user is null || !await userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
                // returned a 400 for an invalid code given a valid user email.
                return Utilities.EndPointUtility.CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
            }

            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
                result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }

            if (!result.Succeeded)
            {
                return Utilities.EndPointUtility.CreateValidationProblem(result);
            }

            return TypedResults.Ok();
        });
    }
}
