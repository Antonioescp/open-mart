using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenMart.Users.Data.Model;

[Table("Users")]
[Index(nameof(Email), IsUnique = true, Name = "IX_Users_Email")]
[Index(nameof(Username), IsUnique = true, Name = "IX_Users_Username")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(30)]
    [Required]
    public string Username { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string PasswordHash { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string PasswordSalt { get; set; } = null!;

    [MaxLength(320)]
    [Required]
    public string Email { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public bool IsEmailConfirmed { get; set; }
    
    [Required]
    [DefaultValue(false)]
    public bool IsLocked { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public short InvalidLoginAttempts { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public bool CanLogin => !this.IsLocked;

    [NotMapped]
    public string FullName => $"{this.FirstName} {this.LastName}";

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