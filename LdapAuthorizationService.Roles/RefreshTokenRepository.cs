using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LdapAuthorizationService.Repository.DbContext;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Internal;

namespace LdapAuthorizationService.Users
{
	public class RefreshTokenRepository : IRefreshTokenRepository
	{
		private AuthServiceDbContext _dbContext;
		public RefreshTokenRepository(AuthServiceDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task SaveRefreshToken(DbOAuthRefreshToken token)
		{
			_dbContext.DbOAuthRefreshTokens.Add(token);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<DbOAuthRefreshToken> GetRefreshToken(string login)
		{
			var result = await _dbContext.DbOAuthRefreshTokens.FirstOrDefaultAsync(r => r.Login.ToLower() == login.ToLower());
			return result;
		}

		public async Task DeleteRefreshToken(string id)
		{
			var token = await FindOAuthRefreshToken(id);

			_dbContext.DbOAuthRefreshTokens.Remove(token);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<DbOAuthRefreshToken> FindOAuthRefreshToken(string id)
		{
			var result = await _dbContext.DbOAuthRefreshTokens.FirstOrDefaultAsync(r => r.OAuthRefreshTokenId == id);
			return result;
		}

		public async Task RemoveOAuthRefreshToken(string id)
		{
			var refreshToken = await _dbContext.DbOAuthRefreshTokens.FirstOrDefaultAsync(r => r.OAuthRefreshTokenId == id);
			if (refreshToken == null)
				return;

			_dbContext.DbOAuthRefreshTokens.Remove(refreshToken);
			await _dbContext.SaveChangesAsync();
		}
	}
}
