using Microsoft.AspNetCore.Identity;

namespace Donut.MemberShip.Core.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; } = new();
}
