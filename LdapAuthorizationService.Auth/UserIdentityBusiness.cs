using System.Linq;
using System.Security.Claims;

namespace LdapAuthorizationService.Auth
{
	public static class UserIdentityBusiness
	{
		public static string GetCustomerRefreshToken(ClaimsPrincipal claimsPrincipal)
		{
			if (claimsPrincipal == null || claimsPrincipal.Claims == null || !claimsPrincipal.Claims.Any())
				return null;

			var refreshTokenId = claimsPrincipal.Claims.First(x => x.Type == "refreshTokenId")?.Value;

			return refreshTokenId;
		}
	}
}
