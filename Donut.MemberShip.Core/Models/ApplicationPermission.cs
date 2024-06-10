namespace Donut.MemberShip.Core.Models;

public class ApplicationPermission : CustomIdentityPermission
{
    public virtual ICollection<ApplicationRolePermission> RolePermissions { get; set; } = [];
}
