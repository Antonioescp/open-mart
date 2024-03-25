using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenMart.Email.Data.Model;

[Table("EmailTemplates")]
[Index(nameof(EmailSubjectId), nameof(Type), IsUnique = true)]
public class EmailTemplate
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey(nameof(EmailSubjectId))]
    public EmailSubject EmailSubject { get; set; } = null!;
    public long EmailSubjectId { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = null!;

    [MaxLength]
    public string Content { get; set; } = null!;
}