using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;

namespace LdapAuthorizationService.Auth.Exceptions
{
	/// <summary>
	/// Ошибка "некорректное имя пользователя или пароль"
	/// </summary>
	public class IncorrectUserNameOrPasswordException : BackendException
	{
		public IncorrectUserNameOrPasswordException(string message = null) : base((int)UserErrorCodes.IncorrectUserNameOrPassword, message) { }
	}
}
