using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Donut.MemberShip.Core.DatabaseContext;
using Donut.MemberShip.Core.Models;
using Donut.SharedKernel.DependencyInjection.Installer;
using Microsoft.Extensions.Configuration;

namespace Donut.MemberShip.Authentication.Configuration;

public class AuthenticationServiceInstaller : IDependencyInjectionInstaller
{
    public int PriorityLevel => 1;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddAuthorizationBuilder();
        //services.AddIdentityApiEndpoints<ApplicationUser>()
        //            .AddEntityFrameworkStores<MemberShipDbContext>();

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<MemberShipDbContext>()
            .AddApiEndpoints();
    }
}
