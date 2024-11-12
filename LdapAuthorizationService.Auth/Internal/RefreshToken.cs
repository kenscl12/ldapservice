using System;

namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Представление refresh token (OAuth2)
	/// </summary>
	public class RefreshToken
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Идентификатор пользователя
		/// </summary>
		public Guid? UserId { get; set; }

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
}
