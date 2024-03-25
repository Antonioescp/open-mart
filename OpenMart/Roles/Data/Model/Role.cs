using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenMart.ExtraSharp.Metadata;
using OpenMart.Users.Data.Model;

namespace OpenMart.Roles.Data.Model;

[Table("Roles")]
public class Role
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)] public string Name { get; set; } = null!;
    [MaxLength(500)] public string Description { get; set; } = null!;
    
    public Timestamp Timestamp { get; set; } = new();
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    [NotMapped] public ICollection<User> Users => this.UserRoles.Select(userRole => userRole.User).ToList();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    [NotMapped]
    public ICollection<Permission> Permissions =>
        this.RolePermissions.Select(rolePermission => rolePermission.Permission).ToList();
}