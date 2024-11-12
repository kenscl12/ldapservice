using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Ldap.Internal;
using LdapAuthorizationService.Users.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LdapAuthorizationService.Middleware
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;

		/// <summary>
		/// Настройки сервиса аутентификации
		/// </summary>
		private AuthentificationSettings _authentificationSettings;

		public JwtMiddleware(RequestDelegate next, AuthentificationSettings authentificationSettings)
		{
			_next = next;
			_authentificationSettings = authentificationSettings;
		}

		public async Task Invoke(HttpContext context, IUserService userService, ILdapUserManager ldapUserManager)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (token != null)
				await AttachUserToContext(context, userService, ldapUserManager, token);

			await _next(context);
		}

		private async Task AttachUserToContext(HttpContext context, IUserService userService, ILdapUserManager ldapUserManager, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_authentificationSettings.JwtTokenSecret);
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userName = jwtToken.Claims.First(x => x.Type == "name").Value;

				var userRoles = await GetUserRoles(userService, userName);

				context.Items["User"] = new UserIdentity
				{
					Name = userName,
					Role = userRoles
				};
			}
			catch
			{
			}
		}

		/// <summary>
		/// Возвращает имена групп
		/// </summary>
		/// <param name="groups">Группы</param>
		private List<string> GetLdapUserGroupNames(IEnumerable<string> groups)
		{
			var groupNames = new List<string>();
			foreach (var group in groups)
			{
				var groupName = GetCnGroupName(group);
				if (!groupNames.Contains(groupName))
					groupNames.Add(groupName);
			}

			return groupNames;
		}

		/// <summary>
		/// Возвращает роли пользователя
		/// </summary>
		/// <param name="userService">IUserService</param>
		/// <param name="userName">Имя пользователя</param>
		private async Task<List<string>> GetUserRoles(IUserService userService, string userName)
		{
			var user = await userService.GetUser(userName);
			if (user == null)
				return new List<string>();

			return user.Roles.Select(role => role.Name).ToList();
		}

		/// <summary>
		/// Возвращает CN секцию имени группы
		/// </summary>
		/// <param name="groupFullName">Полное название группы вида CN=Foo Group Name,DC=mydomain,DC=com</param>
		private string GetCnGroupName(string groupFullName)
		{
			if (string.IsNullOrEmpty(groupFullName))
				return null;

			var splitedGroupFullName = groupFullName.Split(',');
			foreach (string groupFullNameSection in splitedGroupFullName)
			{
				string[] keyValue = groupFullNameSection.Split('=');
				if (keyValue[0] == "CN")
					return keyValue[1];
			}

			return null;
		}
	}
}
