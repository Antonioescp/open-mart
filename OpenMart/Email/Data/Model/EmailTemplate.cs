using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenMart.Email.Data.Model;

[Table("EmailTemplate")]
public class EmailTemplate
{
    [Key]
    public long Id { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = null!;

    [MaxLength(200)]
    public string TemplateName { get; set; } = null!;

    [MaxLength(500)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "NVARCHAR(MAX)")]
    public string Template { get; set; } = null!;
}