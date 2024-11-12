using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace LdapAuthorizationService.Repository.Entities
{
    public class DbRole
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public ICollection<DbUser2Role> User2Roles { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<DbRole>
    {
        public void Configure(EntityTypeBuilder<DbRole> builder)
        {
            builder.ToTable("roles");
            
            builder.HasKey(t => t.Id);

            builder.Property(y => y.Id)
                .HasColumnName("id");

            builder.Property(y => y.Name)
                .IsRequired()
                .HasColumnType("varchar(256)")
                .HasColumnName("name");
            
            builder.Property(y => y.CreatedDate)
                .HasColumnName("created_date");

            builder.Property(y => y.ModifiedDate)
                .HasColumnName("modified_date");

            builder.HasData(
                new DbRole
                {
                    Id = new Guid("0fd161a1-7d3a-4cbb-a5b2-8bf58d78f88e"),
                    Name = "admin"
                },
                new DbRole
                {
                    Id = new Guid("9b15b2a0-8b47-4efa-8bb2-8d10d243e7f4"),
                    Name = "manager"
                }
            );
        }
    }
}
