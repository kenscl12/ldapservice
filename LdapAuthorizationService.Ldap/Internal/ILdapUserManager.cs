using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LdapAuthorizationService.Ldap.Internal
{
	/// <summary>
	/// Интерфейс Ldap сервиса для работы с пользователями
	/// </summary>
	public interface ILdapUserManager : IDisposable
	{
		/// <summary>
		/// Аутентификация
		/// </summary>
		Task<bool> Authenticate(string distinguishedName, string password);

		/// <summary>
		/// Получает пользователя по имени
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		Task<LdapUser> GetUserByUserName(string userName);

		/// <summary>
		/// Получает пользователя по электронной почте
		/// </summary>
		/// <param name="email">Электронная почта</param>
		Task<LdapUser> GetUserByEmail(string email);

		/// <summary>
		/// Получает пользователей по имени
		/// </summary>
		/// <param name="userName">Имя</param>
		/// <param name="limit">Количество вариантов</param>
		Task<List<string>> SearchUsers(string userName, int limit = 7);
	}
}
