using Microsoft.AspNetCore.Identity;

namespace Donut.MemberShip.Core.Models;

public class ApplicationUserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; } = new();
    public virtual ApplicationRole Role { get; set; } = new();
}
