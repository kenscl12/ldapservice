using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LdapAuthorizationService.Repository.Entities
{
	public class DbUser2Role
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public DbUser User { get; set; }

		public Guid RoleId { get; set; }

		public DbRole Role { get; set; }
	}

	public class User2RoleConfiguration : IEntityTypeConfiguration<DbUser2Role>
	{
		public void Configure(EntityTypeBuilder<DbUser2Role> builder)
		{
			builder.ToTable("user2roles");
			
			builder.HasKey(x => new { x.UserId, x.RoleId });

			builder.Property(x => x.Id)
				.HasColumnName("id");

			builder.Property(y => y.UserId)
				.HasColumnName("user_id");

			builder.Property(y => y.RoleId)
				.HasColumnName("role_id");

			builder
				.HasOne(bc => bc.Role)
				.WithMany(b => b.User2Roles)
				.HasForeignKey(bc => bc.RoleId);

			builder
				.HasOne(bc => bc.User)
				.WithMany(b => b.User2Roles)
				.HasForeignKey(bc => bc.UserId);
		}
	}
}
