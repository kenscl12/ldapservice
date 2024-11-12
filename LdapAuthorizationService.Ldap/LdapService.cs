using LdapForNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LdapAuthorizationService.Ldap.Internal;
using static LdapForNet.Native.Native;

namespace LdapAuthorizationService.Ldap
{
    /// <summary>
    /// Ldap сервис
    /// </summary>
    public class LdapService : ILdapService
    {
        /// <summary>
        /// Конфигурация Ldap
        /// </summary>
        private LdapSettings _ldapSettings;

        /// <summary>
        /// Атрибуты поиска
        /// </summary>
        private string[] _searchAttributes =
        {
            "objectSid", "objectGUID", "objectCategory", "objectClass", "memberOf", "name", "cn", "distinguishedName",
            "sAMAccountName", "sAMAccountName", "userPrincipalName", "displayName", "givenName", "sn", "description",
            "telephoneNumber", "mail", "streetAddress", "postalCode", "l", "st", "co", "c"
        };

        /// <summary>
        /// Конструктор
        /// </summary>
        public LdapService(LdapSettings ldapSettings)
        {
            _ldapSettings = ldapSettings;
        }

        /// <summary>
        /// Аутентификация
        /// </summary>
        /// <param name="distinguishedName">Имя и доменная информация о Ldap учетке</param>
        /// <param name="password">Пароль</param>
        public async Task<bool> Authenticate(string distinguishedName, string password)
        {
            using (var connection = new LdapConnection())
            {
                connection.Connect(_ldapSettings.ServerName, _ldapSettings.ServerPort);

                try
                {
                    await connection.BindAsync(LdapAuthMechanism.SIMPLE, distinguishedName, password);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Ищет пользователя по имени
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        public async Task<LdapEntry> SearchUserByUserName(string userName)
        {
            var userNameWithoutDomain = RemoveDomainFromLogin(userName);

            var searchFilter = $"(&(objectClass=user)(sAMAccountName={userNameWithoutDomain}))";

            var searchResults = await Search(searchFilter);

            return searchResults.Single();
        }

        /// <summary>
        /// Ищет пользователя по почте
        /// </summary>
        /// <param name="email">Электронная почта</param>
        public async Task<LdapEntry> SearchUserByEmail(string email)
        {
            var searchFilter = $"(&(objectClass=user)(mail={email}))";

            var searchResults = await Search(searchFilter);

            return searchResults.Single();
        }

        /// <summary>
        /// Ищет пользователей по имени
        /// </summary>
        /// <param name="userName">Имя</param>
        public async Task<IList<LdapEntry>> SearchUsers(string userName)
        {
            var searchFilter = $"(&(objectClass=user)(sAMAccountName={userName}*))";

            var searchResults = await Search(searchFilter);

            return searchResults;
        }

        public async Task<ICollection<LdapEntry>> GetGroups(string groupName)
        {
            var searchFilter = $"(&(objectClass=group)(cn={groupName}*))";

            var searchResults = await Search(searchFilter);

            return searchResults;
        }

        /// <summary>
        /// Поиск по фильтру
        /// </summary>
        /// <param name="searchFilter">Фильтр поиска</param>
        private async Task<IList<LdapEntry>> Search(string searchFilter)
        {
            var connection = await GetConnection();
            var searchResults = await connection.SearchAsync("ou=Departments,dc=company,dc=local", searchFilter, _searchAttributes,
                    LdapSearchScope.LDAP_SCOPE_SUB);

            if (searchResults == null || !searchResults.Any())
                return null;

            return searchResults;
        }

        /// <summary>
        /// Возвращает соединение
        /// </summary>
        private async Task<ILdapConnection> GetConnection()
        {
            var connection = new LdapConnection();

            connection.Connect(_ldapSettings.ServerName, _ldapSettings.ServerPort);

            await connection.BindAsync(LdapAuthMechanism.SIMPLE, _ldapSettings.Login, _ldapSettings.Password);

            return connection;
        }

        /// <summary>
        /// Вырезает домен из имени пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        private string RemoveDomainFromLogin(string userName)
        {
            if (!userName.Contains("\\"))
                return userName;

            return userName.Substring(userName.IndexOf('\\') + 1);
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
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