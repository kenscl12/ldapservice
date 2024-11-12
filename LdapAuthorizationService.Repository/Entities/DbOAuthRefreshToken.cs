using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace LdapAuthorizationService.Repository.Entities
{
	/// <summary>
	/// Представление refresh token (OAuth2)
	/// </summary>
	public class DbOAuthRefreshToken
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public string OAuthRefreshTokenId { get; set; }

		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Защищенный тикет
		/// </summary>
		public string ProtectedTicket { get; set; }

		/// <summary>
		/// Дата выдачи
		/// </summary>
		public DateTime IssuedUtc { get; set; }

		/// <summary>
		/// Дата истечения
		/// </summary>
		public DateTime ExpiresUtc { get; set; }
	}

	public class DbOAuthRefreshTokenConfiguration : IEntityTypeConfiguration<DbOAuthRefreshToken>
	{
		public void Configure(EntityTypeBuilder<DbOAuthRefreshToken> builder)
		{
			builder.ToTable("DbOAuthRefreshToken");

			builder.HasKey(x => x.OAuthRefreshTokenId);

			builder.Property(y => y.OAuthRefreshTokenId)
				.HasColumnName("o_auth_refresh_token_id");

			builder.Property(y => y.Login)
				.HasColumnName("login");

			builder.Property(y => y.ProtectedTicket)
				.HasColumnName("protected_ticket");

			builder.Property(y => y.IssuedUtc)
				.HasColumnName("issued_utc");

			builder.Property(y => y.ExpiresUtc)
				.HasColumnName("expires_utc");
		}
	}
}
