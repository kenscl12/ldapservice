using System;
using System.Threading.Tasks;
using LdapAuthorizationService.Repository.Entities;

namespace LdapAuthorizationService.Users.Internal
{
	public interface IRefreshTokenRepository
	{
		Task SaveRefreshToken(DbOAuthRefreshToken token);

		Task<DbOAuthRefreshToken> GetRefreshToken(string login);

		Task DeleteRefreshToken(string id);

		Task<DbOAuthRefreshToken> FindOAuthRefreshToken(string id);

		Task RemoveOAuthRefreshToken(string id);
	}
}
