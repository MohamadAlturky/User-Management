namespace Donut.MemberShip.Core.Models;

public class ApplicationRolePermission
{
    public int Id { get; set; }
    public string RoleId { get; set; } = string.Empty;
    public string PermissionId { get; set; } = string.Empty;

    public ApplicationPermission Permission { get; set; } = new();
    public ApplicationRole Role { get; set; } = new();
}
