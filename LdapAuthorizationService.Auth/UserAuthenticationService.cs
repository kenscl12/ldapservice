using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using LdapAuthorizationService.Auth.Exceptions;
using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Common.Exceptions;
using LdapAuthorizationService.Common.Helpers;
using LdapAuthorizationService.Ldap.Internal;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Internal;

namespace LdapAuthorizationService.Auth
{
	/// <summary>
	/// Сервис авторизации
	/// </summary>
	public class UserAuthenticationService : IUserAuthenticationService
	{
		/// <summary>
		/// Ldap сервис для работы с пользователями
		/// </summary>
		private ILdapUserManager _ldapUserManager;

		/// <summary>
		/// Сервис пользователей
		/// </summary>
		private IUserService _userService;

		/// <summary>
		/// Настройки сервиса аутентификации
		/// </summary>
		private AuthentificationSettings _authentificationSettings;

		/// <summary>
		/// Репозиторий токенов
		/// </summary>
		private IRefreshTokenRepository _refreshTokenRepository;

		private readonly ILogger<UserAuthenticationService> _logger;

		/// <summary>
		/// Конструктор
		/// </summary>
		public UserAuthenticationService(ILdapUserManager ldapUserManager, AuthentificationSettings authentificationSettings,
			IUserService userService, IRefreshTokenRepository refreshTokenRepository, ILogger<UserAuthenticationService> logger) {
			_ldapUserManager = ldapUserManager;
			_authentificationSettings = authentificationSettings;
			_userService = userService;
			_refreshTokenRepository = refreshTokenRepository;
			_logger = logger;
		}

		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		/// <param name="model">Представление авторизации пользователя</param>
		public async Task<JObject> SignInAsync(SignInUser model)
		{
			if (model == null || string.IsNullOrEmpty((model.Grant_Type)))
				throw new BackendException("auth parameters required");

			switch (model.Grant_Type.ToLower())
			{
				case "password":
					return await SignInAsync(model.Login, model.Password);
				case "refresh_token":
					return await SignInAsync(model.RefreshToken);
				default:
					throw new BackendException("invalid granttype");
			}
		}

		/// <summary>
		/// Logout пользователя
		/// </summary>
		/// <param name="ClaimsPrincipal">claims</param>
		public async Task Logout(ClaimsPrincipal claims)
		{
			var refreshToken = UserIdentityBusiness.GetCustomerRefreshToken(claims);

			var hashedTokenId = CryptographyHelper.ComputeSha256Hash(refreshToken);

			await _refreshTokenRepository.RemoveOAuthRefreshToken(hashedTokenId);
		}

		/// <summary>
		/// Авторизация пользователя по refreshToken
		/// </summary>
		/// <param name="refreshToken">refreshToken</param>
		private async Task<JObject> SignInAsync(string refreshToken)
		{
			var hashedTokenId = CryptographyHelper.ComputeSha256Hash(refreshToken);
			var value = await _refreshTokenRepository.FindOAuthRefreshToken(hashedTokenId);
			if (value == null)
				throw new IncorrectTokenException("Некорректный токен");

			var ldapUser = await _ldapUserManager.GetUserByUserName(value.Login);
			if (ldapUser == null)
				throw new UserNotFoundException("Пользователь не найден");

			var user = await _userService.GetUser(ldapUser.SamAccountName);
			if (user == null)
				throw new UserNotFoundException("Пользователь не найден");

			return await GenerateAuthentificationResponse(user, ldapUser);
		}

		/// <summary>
		/// Авторизация пользователя по имени и паролю
		/// </summary>
		/// <param name="login">Логин</param>
		/// <param name="password">Пароль</param>
		private async Task<JObject> SignInAsync(string login, string password)
		{
			try
			{
				LdapUser ldapUser;
				if (IsEmailLogin(login))
					ldapUser = await _ldapUserManager.GetUserByEmail(login);
				else
					ldapUser = await _ldapUserManager.GetUserByUserName(login);

				if (ldapUser == null)
					throw new UserNotFoundException("Пользователь не найден");

				if (string.IsNullOrEmpty(password) || !await _ldapUserManager.Authenticate(ldapUser.DistinguishedName, password))
					throw new IncorrectUserNameOrPasswordException();

				var user = await _userService.GetUser(ldapUser.SamAccountName);
				if (user == null)
					throw new UserNotFoundException("Пользователь не найден");

				if (user.Status?.Code == nameof(UserStatus.Disabled))
					throw new InvalidAccessException();

				return await GenerateAuthentificationResponse(user, ldapUser);
			}

			catch (IncorrectUserNameOrPasswordException)
			{
				throw new IncorrectUserNameOrPasswordException("Auth error incorrect userName or password");
			}
			catch (InvalidAccessException)
			{
				throw new InvalidAccessException("Доступ запрещен");
			}
			catch (Exception ex)
			{
				throw new BackendException("Auth error: " + ex.Message);
			}
		}

		/// <summary>
		/// Генерирует токен с информацией об авторизации
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="ldapUser">Ldap пользователь</param>
		private async Task<JObject> GenerateAuthentificationResponse(User user, LdapUser ldapUser)
		{
			var refreshToken = await CreateRefreshToken(ldapUser.SamAccountName);
			var token = GenerateJwtToken(ldapUser, user, refreshToken.Id);

			return new JObject(
				new JProperty("access_token", token),
				new JProperty("token_type", "bearer"),
				new JProperty("expires_in", GetAccessTokenLifeTime()),
				new JProperty("refresh_token", refreshToken.Id)
			);
		}

		/// <summary>
		/// Создать refreshToken токен
		/// </summary>
		/// <param name="userId">Идентификатор пользователя</param>
		private async Task<RefreshToken> CreateRefreshToken(string login)
		{
			var existToken = await _refreshTokenRepository.GetRefreshToken(login);
			if (existToken != null)
				await _refreshTokenRepository.DeleteRefreshToken(existToken.OAuthRefreshTokenId);

			var refreshTokenId = Guid.NewGuid().ToString("n");
			var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			var randomBytes = new byte[64];
			rngCryptoServiceProvider.GetBytes(randomBytes);
			var refreshToken = new RefreshToken
			{
				Id = refreshTokenId,
				Login = login,
				ProtectedTicket = Convert.ToBase64String(randomBytes),
				IssuedUtc = DateTime.UtcNow,
				ExpiresUtc = DateTime.UtcNow.AddSeconds(_authentificationSettings.RefreshTokenLifeTime)
			};

			await _refreshTokenRepository.SaveRefreshToken(new DbOAuthRefreshToken
			{
				Login = login,
				ExpiresUtc = refreshToken.ExpiresUtc,
				ProtectedTicket = refreshToken.ProtectedTicket,
				IssuedUtc = refreshToken.IssuedUtc,
				OAuthRefreshTokenId = CryptographyHelper.ComputeSha256Hash(refreshToken.Id)
			});

			return refreshToken;
		}

		/// <summary>
		/// Генерация JWT токена
		/// </summary>
		/// <param name="ldapUser">LDAP пользовалель</param>
		/// <param name="user">Пользователь</param>
		/// <param name="refreshToken">refreshToken</param>
		private string GenerateJwtToken(LdapUser ldapUser, User user, string refreshToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authentificationSettings.JwtTokenSecret));
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = GetClaimsIdentity(ldapUser, user, refreshToken),
				Expires = DateTime.UtcNow.AddSeconds(_authentificationSettings.AccessTokenLifeTime),
				SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		/// <summary>
		/// Получить удостоверение
		/// </summary>
		/// <param name="ldapUser">Ldap пользовалель</param>
		/// <param name="user">Пользователь</param>
		/// <param name="refreshToken">refreshToken</param>
		private ClaimsIdentity GetClaimsIdentity(LdapUser ldapUser, User user, string refreshToken)
		{
			var result = new ClaimsIdentity();

			result.AddClaim(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
			result.AddClaim(new Claim("userType", "user"));
			result.AddClaim(new Claim(JwtClaimTypes.Name, ldapUser.SamAccountName));
			
			if (IsEmailLogin(ldapUser.Email))
			{
				result.AddClaim(new Claim(JwtClaimTypes.Email, ldapUser.Email));
			}
			else if (IsEmailLogin(ldapUser.UserPrincipalName))
			{
				result.AddClaim(new Claim(JwtClaimTypes.Email, ldapUser.UserPrincipalName));
			}
			result.AddClaim(new Claim("sub", ldapUser.SamAccountName));
			result.AddClaim(new Claim("refreshTokenId", refreshToken));

			foreach (var role in user.Roles)
			{
				result.AddClaim(new Claim("role", role.Name));
			}

			return result;
		}

		/// <summary>
		/// Получение времени жизни токена
		/// </summary>
		private double GetAccessTokenLifeTime()
		{
			var expiresDate = DateTime.UtcNow.AddSeconds(_authentificationSettings.AccessTokenLifeTime);
			return (int)expiresDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
		}

		/// <summary>
		/// Возвращает признак что логин является email-ом
		/// </summary>
		/// <param name="login">Логин</param>
		private static bool IsEmailLogin(string email)
		{
			if (string.IsNullOrEmpty(email))
				return false;
            
			try
			{
				var mailAddress = new MailAddress(email);
				return true;
			}
			catch
			{
				return false;
			}
		}


		#region IDisposable Support

		private bool _disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (_disposedValue) return;

			if (disposing)
			{
				_ldapUserManager.Dispose();
			}

			_disposedValue = true;
		}

		void System.IDisposable.Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
