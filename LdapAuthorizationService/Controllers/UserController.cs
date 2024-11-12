using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;
using LdapAuthorizationService.Ldap.Internal;
using LdapAuthorizationService.Users.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LdapAuthorizationService.Controllers
{
	[Route("api/user")]
	public class UserController : Controller
	{
		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private IUserAuthenticationService _userAuthenticationService;

		//// <summary>
		/// Интерфейс Ldap сервиса для работы с пользователями
		/// </summary>
		private ILdapUserManager _ldapUserManager;

		private readonly IUserService _userService;

		public UserController(IUserAuthenticationService userAuthenticationService, ILdapUserManager ldapUserManager, IUserService userService)
		{
			_userAuthenticationService = userAuthenticationService;
			_ldapUserManager = ldapUserManager;
			_userService = userService;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> SignIn([FromBody] SignInUser model)
		{
			var result = await _userAuthenticationService.SignInAsync(model);

			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> GetUserInfo()
		{
			var userIdentity = HttpContext.Items["User"] as UserIdentity;

			if (userIdentity == null)
				throw new BackendException();

			return Ok(userIdentity);
		}

		[HttpGet("search/{q}")]
		[Authorize]
		public async Task<IActionResult> AutocompleteUsers(string q, int limit = 7)
		{
			var result = await _ldapUserManager.SearchUsers(q, limit);

			return Ok(result);
		}

		[HttpPost("logout")]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _userAuthenticationService.Logout(this.User);
			return Ok();
		}
	}
}
