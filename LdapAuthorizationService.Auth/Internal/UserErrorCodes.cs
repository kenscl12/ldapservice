namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Коды ошибок
	/// </summary>
	public enum UserErrorCodes
	{
		/// <summary>
		/// Некорректный email или пароль
		/// </summary>
		IncorrectUserNameOrPassword = 100,

		/// <summary>
		/// Некорректный токен
		/// </summary>
		IncorrectToken = 101,

		/// <summary>
		/// Юзер не найден
		/// </summary>
		UserNotFoundException = 102,

		/// <summary>
		/// Доступ запрещен
		/// </summary>
		InvalidAccess = 103
	}
}
