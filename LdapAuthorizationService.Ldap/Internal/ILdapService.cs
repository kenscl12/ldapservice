using LdapForNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LdapAuthorizationService.Ldap.Internal
{
	/// <summary>
	/// Интерфейс Ldap сервиса
	/// </summary>
	public interface ILdapService : IDisposable
	{
		/// <summary>
		/// Аутентификация
		/// </summary>
		Task<bool> Authenticate(string distinguishedName, string password);

		/// <summary>
		/// Ищет пользователя по имени
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		Task<LdapEntry> SearchUserByUserName(string userName);

		/// <summary>
		/// Ищет пользователя по почте
		/// </summary>
		/// <param name="email">Электронная почта</param>
		Task<LdapEntry> SearchUserByEmail(string email);

		/// <summary>
		/// Ищет доменные группы
		/// </summary>
		/// <param name="groupName">Доменная группа</param>
		Task<ICollection<LdapEntry>> GetGroups(string groupName);

		/// <summary>
		/// Ищет пользователей по имени
		/// </summary>
		/// <param name="userName">Имя</param>
		Task<IList<LdapEntry>> SearchUsers(string userName);
	}
}
