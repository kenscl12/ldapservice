using LdapForNet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LdapAuthorizationService.Ldap.Extensions;
using LdapAuthorizationService.Ldap.Internal;

namespace LdapAuthorizationService.Ldap
{
	/// <summary>
	/// Ldap сервис для работы с группами
	/// </summary>
	public class LdapGroupManager : ILdapGroupManager
	{
		/// <summary>
		/// Ldap сервис
		/// </summary>
		private ILdapService _ldapService;

		/// <summary>
		/// Конструктор
		/// </summary>
		public LdapGroupManager(ILdapService ldapService)
		{
			_ldapService = ldapService;
		}

		/// <summary>
		/// Поиск групп по названию
		/// </summary>
		/// <param name="groupName">Имя группы</param>
		/// <param name="limit">Количество вариантов</param>
		public async Task<List<string>> SearchGroups(string groupName, int limit = 7)
		{
			var searchGroups = await _ldapService.GetGroups(groupName);

			if (searchGroups == null)
				return null;

			return searchGroups
				.Select(searchGroup => searchGroup.DirectoryAttributes.GetSafeStringValue("sAMAccountName"))
				.Take(limit)
				.ToList();
		}

		#region IDisposable Support

		private bool _disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (_disposedValue) return;

			if (disposing)
			{
				_ldapService.Dispose();
			}

			_disposedValue = true;
		}

		void System.IDisposable.Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
