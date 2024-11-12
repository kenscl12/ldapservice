using System;

namespace LdapAuthorizationService.Users.Internal
{
	public class UpdateUserRoleRequest
	{

		public Guid[] Roles { get; set; }
	}
}
