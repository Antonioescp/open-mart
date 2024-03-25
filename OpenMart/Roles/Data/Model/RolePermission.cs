using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenMart.ExtraSharp.Metadata;

namespace OpenMart.Roles.Data.Model;

[Table("RolePermissions")]
public class RolePermission
{
    [Key] public int Id { get; set; }

    [ForeignKey(nameof(RoleId))] public Role Role { get; set; } = null!;
    public int RoleId { get; set; }

    [ForeignKey(nameof(PermissionId))] public Permission Permission { get; set; } = null!;
    public int PermissionId { get; set; }

    [DefaultValue(false)] 
    public bool IsEnabled { get; set; }

    public Timestamp Timestamp { get; set; } = new();
}