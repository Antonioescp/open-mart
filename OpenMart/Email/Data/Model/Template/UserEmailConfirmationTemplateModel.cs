using AutoMapper;
using OpenMart.Users.Data.Model;

namespace OpenMart.Email.Data.Model.Template;

public class UserEmailConfirmationTemplateModel
{
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string ConfirmationToken { get; set; } = null!;
}