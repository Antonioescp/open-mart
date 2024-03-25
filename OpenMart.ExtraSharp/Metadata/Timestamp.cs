using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OpenMart.ExtraSharp.Metadata;

[Owned]
public class Timestamp
{
    [Column("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column("UpdatedAt")]
    public DateTime? UpdatedAt { get; set; }

    public void Update() => this.UpdatedAt = DateTime.UtcNow;
}