using System;

namespace LdapAuthorizationService.Users.Internal
{
	public class Role
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
