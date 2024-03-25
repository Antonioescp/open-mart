using Microsoft.EntityFrameworkCore;
using OpenMart.ExtraSharp.Metadata;

namespace OpenMart.Data.Context;

public class OpenMartDbContext : DbContext
{
    public OpenMartDbContext(DbContextOptions<OpenMartDbContext> options) : base(options)
    {
    }

    public DbSet<OpenMart.Users.Data.Model.User> Users { get; set; } = null!;
    public DbSet<OpenMart.Email.Data.Model.EmailSubject> EmailSubjects { get; set; } = null!;
    public DbSet<OpenMart.Email.Data.Model.EmailTemplate> EmailTemplates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(OpenMartDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new TimestampInterceptor());
    }
}