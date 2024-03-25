using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenMart.Email.Data.Model;

[Table("EmailSubjects")]
[Index(nameof(Name), IsUnique = true)]
public class EmailSubject
{
    [Key]
    public long Id { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [MaxLength(200)]
    public string Subject { get; set; } = null!;

    public ICollection<EmailTemplate> EmailTemplates { get; set; } = new List<EmailTemplate>();
}