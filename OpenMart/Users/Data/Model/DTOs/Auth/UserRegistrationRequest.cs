using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMart.Users.Data.Model.DTOs;

public class UserRegistrationRequest
{
    [MaxLength(30)]
    [Required]
    public string Username { get; set; } = null!;
    
    [MaxLength(100)]
    [Required]
    public string Password { get; set; } = null!;

    [MaxLength(320)]
    [Required]
    public string Email { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    [Required]
    public string LastName { get; set; } = null!;
}