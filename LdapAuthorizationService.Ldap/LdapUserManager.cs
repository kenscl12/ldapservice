using LdapForNet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LdapAuthorizationService.Ldap.Extensions;
using LdapAuthorizationService.Ldap.Internal;

namespace LdapAuthorizationService.Ldap
{
	/// <summary>
	/// Ldap сервис для работы с пользователями
	/// </summary>
	public class LdapUserManager : ILdapUserManager
	{
		/// <summary>
		/// Ldap сервис
		/// </summary>
		private ILdapService _ldapService;

		/// <summary>
		/// Конструктор
		/// </summary>
		public LdapUserManager(ILdapService ldapService)
		{
			_ldapService = ldapService;
		}

		/// <summary>
		/// Аутентификация
		/// </summary>
		/// <param name="distinguishedName">Имя и доменная информация о Ldap учетке</param>
		/// <param name="password">Пароль</param>
		public async Task<bool> Authenticate(string distinguishedName, string password)
		{
			return await _ldapService.Authenticate(distinguishedName, password);
		}

		/// <summary>
		/// Получает пользователя по имени
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		public async Task<LdapUser> GetUserByUserName(string userName)
		{
			var searchUser = await _ldapService.SearchUserByUserName(userName);

			if (searchUser == null)
				return null;

			var ldapUser = CreateLdapUserFromAttributes(searchUser.DirectoryAttributes);

			return ldapUser;
		}

		/// <summary>
		/// Получает пользователя по электронной почте
		/// </summary>
		/// <param name="email">Электронная почта</param>
		public async Task<LdapUser> GetUserByEmail(string email)
		{
			var searchUser = await _ldapService.SearchUserByEmail(email);

			if (searchUser == null)
				return null;

			var ldapUser = CreateLdapUserFromAttributes(searchUser.DirectoryAttributes);

			return ldapUser;
		}

		/// <summary>
		/// Получает пользователей по имени
		/// </summary>
		/// <param name="userName">Имя</param>
		/// <param name="limit">Количество вариантов</param>
		public async Task<List<string>> SearchUsers(string userName, int limit = 7)
		{
			var searchUsers = await _ldapService.SearchUsers(userName);

			if (searchUsers == null || !searchUsers.Any())
				return null;

			return searchUsers
				.Select(searchGroup => searchGroup.DirectoryAttributes.GetSafeStringValue("sAMAccountName"))
				.Take(limit)
				.ToList();
		}

		/// <summary>
		/// Создает пользователя из атрибутов
		/// </summary>
		/// <param name="attributes">Атрибуты</param>
		private LdapUser CreateLdapUserFromAttributes(SearchResultAttributeCollection attributes)
		{
			var ldapUser = new LdapUser
			{
				MemberOf = attributes["memberOf"].GetValues<string>(),
				FullName = attributes.GetSafeStringValue("name"),
				SamAccountName = attributes.GetSafeStringValue("sAMAccountName"),
				UserPrincipalName = attributes.GetSafeStringValue("userPrincipalName"),
				DistinguishedName = attributes.GetSafeStringValue("distinguishedName"),
				DisplayName = attributes.GetSafeStringValue("displayName"),
				FirstName = attributes.GetSafeStringValue("givenName"),
				LastName = attributes.GetSafeStringValue("sn"),
				Description = attributes.GetSafeStringValue("description"),
				Phone = attributes.GetSafeStringValue("telephoneNumber"),
				Email = attributes.GetSafeStringValue("mail")
			};

			return ldapUser;
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
