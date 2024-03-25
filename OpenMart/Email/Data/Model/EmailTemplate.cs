using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMart.Email.Data.Model;

[Table("EmailTemplates")]
public class EmailTemplate
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey(nameof(EmailSubject))]
    public long EmailSubjectId { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = null!;

    [MaxLength]
    public string Template { get; set; } = null!;
    
    // Nav properties
    public EmailSubject EmailSubject { get; set; } = null!;
}