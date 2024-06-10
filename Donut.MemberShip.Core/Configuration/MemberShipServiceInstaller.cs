using Microsoft.Extensions.DependencyInjection;
using Donut.MemberShip.Core.DatabaseContext;
using Donut.SharedKernel.DependencyInjection.Installer;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Donut.MemberShip.Core.Configuration;

public class MemberShipServiceInstaller : IDependencyInjectionInstaller
{
    private readonly string SECTION_NAME = "MemberShipConnection";

    public int PriorityLevel => 0;

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<MemberShipDbContext>(x=>x.UseSqlServer(configuration.GetConnectionString(SECTION_NAME)));
        services.AddDbContext<MemberShipDbContext>();
    }
}
