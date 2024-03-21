using System.ComponentModel.DataAnnotations;

namespace OpenMart.Users.Data.Model.DTOs.Auth;

public class UserLoginRequest
{
    [Required]
    [MaxLength(320)]
    public string UsernameOrEmail { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}