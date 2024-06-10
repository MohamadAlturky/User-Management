using Donut.MemberShip.Authentication.AssemblyReference;
using Donut.MemberShip.Authentication.EndPoints.Installer;
using Donut.SharedKernel.Utilities;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace Donut.MemberShip.Authentication.EndPoints.Scanner;

public static class EndPointsScanner
{
    public static IEndpointRouteBuilder MapAuthenticationEndPoints(this IEndpointRouteBuilder routeBuilder)
    {
        Assembly[] assemblies = [typeof(AuthenticationAssemblyReference).Assembly];
        var installers = assemblies.SelectMany(assembly => assembly.DefinedTypes)
                  .Where(TypeChecker.IsAssignableToType<IEndPointInstaller>)
                  .Select(Activator.CreateInstance)
                  .Cast<IEndPointInstaller>();
        foreach (var installer in installers)
        {
            installer.AddRoutes(routeBuilder);
        }
        return routeBuilder;
    }
}
