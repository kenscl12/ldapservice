using System;
using System.Collections.Generic;
using System.Text;

namespace LdapAuthorizationService.Users.Internal
{
	public class UsersResponse
	{
		public List<User> Users { get; set; }
		public int PageCount { get; set; }
	}
}
