namespace LdapAuthorizationService.Users.Internal
{
	/// <summary>
	/// Тип пользвателя
	/// </summary>
	public enum UserType
	{
		/// <summary>
		/// Пользователь
		/// </summary>
		User = 1,

		/// <summary>
		/// Доменная группа
		/// </summary>
		DomainGroup = 2
	}
}