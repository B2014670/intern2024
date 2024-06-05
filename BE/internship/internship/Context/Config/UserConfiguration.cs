using internship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace internship.Context.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Configure primary key
            builder.HasKey(u => u.Id);

            // Configure properties
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Address).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();
            builder.Property(u => u.Status).HasDefaultValue(false);
            builder.Property(u => u.RegisteredTime).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(u => u.UpdatedTime)
              .ValueGeneratedOnAddOrUpdate()
              .HasDefaultValueSql("GETUTCDATE()")
              .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        }
    }
}
