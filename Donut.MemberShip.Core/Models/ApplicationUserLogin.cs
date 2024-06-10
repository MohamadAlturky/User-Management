using Microsoft.AspNetCore.Identity;

namespace Donut.MemberShip.Core.Models;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; } = new();
}
