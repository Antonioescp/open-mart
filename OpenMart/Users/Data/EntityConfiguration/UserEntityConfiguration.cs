using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenMart.ExtraSharp.Metadata;
using OpenMart.Users.Data.Model;

namespace OpenMart.Users.Data.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(usr => usr.IsEmailConfirmed)
            .HasDefaultValue(false);
    }
}