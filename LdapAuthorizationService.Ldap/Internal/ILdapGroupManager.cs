using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LdapAuthorizationService.Ldap.Internal
{
	/// <summary>
	/// Интерфейс Ldap сервиса для работы с группамми
	/// </summary>
	public interface ILdapGroupManager : IDisposable
	{
		/// <summary>
		/// Поиск групп по названию
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		/// <param name="limit">Количество вариантов</param>
		Task<List<string>> SearchGroups(string groupName, int limit = 7);
	}
}
