using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LdapAuthorizationService.Auth.Internal
{
	/// <summary>
	/// Интерфейс сервиса авторизации
	/// </summary>
	public interface IUserAuthenticationService : IDisposable
	{
		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		/// <param name="model">Представление авторизации пользователя</param>
		Task<JObject> SignInAsync(SignInUser model);

		/// <summary>
		/// Logout пользователя
		/// </summary>
		/// <param name="ClaimsPrincipal">claims</param>
		Task Logout(ClaimsPrincipal claims);
	}
}