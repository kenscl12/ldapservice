namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Конфигурация аутентификации
	/// </summary>
	public class AuthentificationSettings
	{
		/// <summary>
		/// Ключ для шифрования jwt токена
		/// </summary>
		public string JwtTokenSecret { get; set; }

		/// <summary>
		/// Время жизни accessToken
		/// </summary>
		public double AccessTokenLifeTime { get; set; }

		/// <summary>
		/// Время жизни refreshToken
		/// </summary>
		public double RefreshTokenLifeTime { get; set; }
	}
}
