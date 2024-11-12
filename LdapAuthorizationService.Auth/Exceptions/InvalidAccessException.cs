using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;

namespace LdapAuthorizationService.Auth.Exceptions
{
	/// <summary>
	/// Ошибка "Доступ запрещен"
	/// </summary>
	public class InvalidAccessException : BackendException
	{
		public InvalidAccessException(string message = null) : base((int)UserErrorCodes.InvalidAccess, message) { }
	}
}
