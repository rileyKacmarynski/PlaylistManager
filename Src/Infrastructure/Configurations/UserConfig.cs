using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.SpotifyId).HasMaxLength(50);
            builder.Property(e => e.DisplayName).HasMaxLength(200);

            builder.Property(e => e.Id)
                .UseSqlServerIdentityColumn()
                .HasColumnName("UserId");
        }
    }
}