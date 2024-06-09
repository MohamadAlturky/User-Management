using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Donut.SharedKernel.DependencyInjection.Installer;

public interface IDependencyInjectionInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}
