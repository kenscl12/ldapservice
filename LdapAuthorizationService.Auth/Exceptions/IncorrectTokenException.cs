using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;

namespace LdapAuthorizationService.Auth.Exceptions
{
	/// <summary>
	/// Ошибка "Некорректный токен"
	/// </summary>
	public class IncorrectTokenException : BackendException
	{
		public IncorrectTokenException(string message = null) : base((int)UserErrorCodes.IncorrectToken, message) { }
	}
}
