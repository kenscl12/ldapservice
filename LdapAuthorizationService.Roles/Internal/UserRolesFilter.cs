namespace LdapAuthorizationService.Users.Internal
{
	/// <summary>
	///  Фильтр пользователей
	/// </summary>
	public class UserRolesFilter
	{
		/// <summary>
		///  Страница
		/// </summary>
		public int? Page { get; set; }
		/// <summary>
		///  Размер страницы
		/// </summary>
		public int? PageSize { get; set; }
	}
}
