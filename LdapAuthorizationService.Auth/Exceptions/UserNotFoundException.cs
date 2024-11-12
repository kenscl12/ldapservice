using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;

namespace LdapAuthorizationService.Auth.Exceptions
{
	/// <summary>
	/// Ошибка "пользователь не найден"
	/// </summary>
	public class UserNotFoundException : BackendException
	{
		public UserNotFoundException(string message = null) : base((int)UserErrorCodes.UserNotFoundException, message) { }
	}
}
