using Microsoft.AspNetCore.Routing;

namespace Donut.MemberShip.Authentication.EndPoints.Installer;

public interface IEndPointInstaller
{
    public void AddRoutes(IEndpointRouteBuilder endpoints);
}
