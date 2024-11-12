using System;
using System.Collections.Generic;

namespace LdapAuthorizationService.Users.Internal
{
	public class User
	{
		public Guid Id { get; set; }

		public string Login { get; set; }

		public UserType UserType { get; set; }

		public string ExternalId { get; set; }

		public List<Role> Roles { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }

		public CodeWithDescription Status { get; set; }
	}
}
