using System;
using System.Collections.Generic;

namespace LdapAuthorizationService.Ldap.Internal
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class LdapUser
    {
        /// <summary>
        /// Полное имя
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Имя и доменная информация о Ldap учетке
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Имя учётки в транслите, которое для логина используется (формат: firstname.lastname)
        /// </summary>
        public string SamAccountName { get; set; }

        /// <summary>
        /// Группы пользователя
        /// </summary>
        public IEnumerable<string> MemberOf { get; set; }

        /// <summary>
        /// Признак необходимости смены пароля при следующем логине
        /// </summary>
        public bool MustChangePasswordOnNextLogon { get; set; }

        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Имя для отображения
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        public string SecurityStamp => Guid.NewGuid().ToString("D");
    }
}
