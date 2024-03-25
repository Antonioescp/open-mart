using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using OpenMart.Users.Data.Model;

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
        builder.Entity<User>(typeBuilder =>
        {
            typeBuilder
                .Property(usr => usr.IsEmailConfirmed)
                .HasDefaultValue(false);
            
            typeBuilder
                .Property(usr => usr.CreatedAt)
                .HasDefaultValueSql("getutcdate()");
        });
    }
}