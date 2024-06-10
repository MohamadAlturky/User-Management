using Microsoft.AspNetCore.Identity;

namespace Donut.MemberShip.Core.Models;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } = [];
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = [];
}
