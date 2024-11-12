using System;

namespace LdapAuthorizationService.Users.Internal
{
	public class AddAddRoleToUserRequest
	{
		public string Login { get; set; }

		public UserType UserType { get; set; }

		public string ExternalId { get; set; }

		public Guid[] Roles { get; set; }
	}
}
