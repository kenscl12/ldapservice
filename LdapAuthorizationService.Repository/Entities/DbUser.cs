using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LdapAuthorizationService.Repository.Entities
{
	public class DbUser
	{
		public Guid Id { get; set; }

		public string Login { get; set; }

		public byte UserType { get; set; }

		public string ExternalId { get; set; }
		public ICollection<DbUser2Role> User2Roles { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public string Status { get; set; }
		public DbUserStatus UserStatus { get; set; }
	}

	public class DbUserConfiguration : IEntityTypeConfiguration<DbUser>
	{
		public void Configure(EntityTypeBuilder<DbUser> builder)
		{
			builder.ToTable("users");
			
			builder.Property(y => y.Id)
				.HasColumnName("id");

			builder.Property(y => y.Login)
				.IsRequired()
				.HasColumnType("varchar(256)")
				.HasColumnName("login");

			builder.Property(y => y.UserType)
				.IsRequired()
				.HasColumnName("user_type");

			builder.Property(y => y.ExternalId)
				.HasColumnType("varchar(256)")
				.HasColumnName("external_id");

			builder.Property(y => y.Status)
				.HasColumnType("varchar(256)")
				.HasDefaultValue("Active")
				.HasColumnName("status");

			builder.Property(y => y.CreatedDate)
				.HasColumnName("created_date");

			builder.Property(y => y.ModifiedDate)
				.HasColumnName("modified_date");
		}
	}
}