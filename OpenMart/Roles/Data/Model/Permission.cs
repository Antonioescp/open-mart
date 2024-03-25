using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenMart.ExtraSharp.Metadata;
using OpenMart.Users.Data.Model;

namespace OpenMart.Roles.Data.Model;

[Table("Permissions")]
public class Permission
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(100)] 
    public string Description { get; set; } = null!;

    public Timestamp TimeStamp { get; set; } = new();
    
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    [NotMapped] public ICollection<Role> Roles => this.RolePermissions.Select(rolePermission => rolePermission.Role).ToList();
    [NotMapped] public ICollection<UserRole> UserRoles => this.Roles.SelectMany(ur => ur.UserRoles).ToList();
    [NotMapped] public ICollection<User> Users => this.UserRoles.Select(ur => ur.User).ToList();
}