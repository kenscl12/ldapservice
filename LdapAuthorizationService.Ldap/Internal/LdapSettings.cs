namespace LdapAuthorizationService.Ldap.Internal
{
	/// <summary>
	/// Конфигурация LDAP
	/// </summary>
	public class LdapSettings
	{
		/// <summary>
		/// Название сервера
		/// </summary>
		public string ServerName { get; set; }

		/// <summary>
		/// Порт
		/// </summary>
		public int ServerPort { get; set; }

		/// <summary>
		/// Поисковая база
		/// </summary>
		public string SearchBase { get; set; }

		/// <summary>
		/// Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }
	}
}
