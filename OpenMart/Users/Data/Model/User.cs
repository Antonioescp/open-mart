using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using OpenMart.ExtraSharp.Metadata;
using OpenMart.Roles.Data.Model;

namespace OpenMart.Users.Data.Model;

[Table("Users")]
[Index(nameof(Email), IsUnique = true, Name = "IX_Users_Email")]
[Index(nameof(Username), IsUnique = true, Name = "IX_Users_Username")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(30)] public string Username { get; set; } = null!;
    
    [MaxLength(100)] public string PasswordHash { get; set; } = null!;
    
    [MaxLength(100)] public string PasswordSalt { get; set; } = null!;

    [MaxLength(320)] public string Email { get; set; } = null!;

    [MaxLength(100)] public string FirstName { get; set; } = null!;

    [MaxLength(100)] public string LastName { get; set; } = null!;

    public bool IsEmailConfirmed { get; set; }
    
    [DefaultValue(false)] public bool IsLocked { get; set; }
    [DefaultValue(0)] public short InvalidLoginAttempts { get; set; }
    
    public Timestamp Timestamp { get; set; } = new();
    [NotMapped] public bool CanLogin => !this.IsLocked;
    [NotMapped] public string FullName => $"{this.FirstName} {this.LastName}";
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    [NotMapped] public ICollection<Role> Roles => this.UserRoles.Select(userRole => userRole.Role).ToList();
    [NotMapped] public ICollection<RolePermission> RolePermissions => this.Roles.SelectMany(r => r.RolePermissions).ToList();
    [NotMapped] public ICollection<Permission> Permissions => this.RolePermissions.Select(rp => rp.Permission).ToList();

    public void ResetLockTracking()
    {
        this.IsLocked = false;
        this.InvalidLoginAttempts = 0;
    }

    public void UpdateLockTracking(int maxTries)
    {
        this.InvalidLoginAttempts += 1;
        this.IsLocked = this.InvalidLoginAttempts >= maxTries;
    }
}