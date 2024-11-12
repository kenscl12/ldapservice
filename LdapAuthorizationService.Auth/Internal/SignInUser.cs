namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Представление авторизации пользователя
	/// </summary>
	public class SignInUser
	{
		/// <summary>
		/// Тип авторизации
		/// </summary>
		public string Grant_Type { get; set; }

		/// <summary>
		/// Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// refreshToken
		/// </summary>
		public string RefreshToken { get; set; }
	}
}
