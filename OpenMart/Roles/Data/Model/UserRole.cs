using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenMart.ExtraSharp.Metadata;
using OpenMart.Users.Data.Model;

namespace OpenMart.Roles.Data.Model;

[Table("UserRoles")]
public class UserRole
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set; } = null!;
    public int RoleId { get; set; }
    
    [DefaultValue(false)]
    public bool IsEnabled { get; set; }
    
    public Timestamp Timestamp { get; set; } = new();
}