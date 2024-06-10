using Microsoft.AspNetCore.Identity;

namespace Donut.MemberShip.Core.Models;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = [];
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = [];
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = [];
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
}
