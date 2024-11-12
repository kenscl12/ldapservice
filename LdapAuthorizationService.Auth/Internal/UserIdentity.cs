using System.Collections.Generic;

namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Информация о пользователе
	/// </summary>
	public class UserIdentity
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Роли
		/// </summary>
		public List<string> Role { get; set; }
	}
}
