using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LdapAuthorizationService.Repository.Entities
{
    public class DbUserStatus
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public ICollection<DbUser> DbUsers { get; set; }
    }

    public class DbUserStatusConfiguration : IEntityTypeConfiguration<DbUserStatus>
    {
        public void Configure(EntityTypeBuilder<DbUserStatus> builder)
        {
            builder.ToTable("user_statuses");
            
            builder.HasKey(c => c.Code);

            builder.Property(y => y.Code)
                .HasColumnName("code");

            builder.Property(y => y.Description)
                .HasColumnName("description");

            builder
                .HasMany(f => f.DbUsers)
                .WithOne(b => b.UserStatus)
                .HasForeignKey(f => f.Status);

            builder.HasData(
                new DbUserStatus
                {
                    Code = "Active",
                    Description = "Активен"
                },
                new DbUserStatus
                {
                    Code = "Disabled",
                    Description = "Заблокирован"
                }
            );
        }
    }
}
