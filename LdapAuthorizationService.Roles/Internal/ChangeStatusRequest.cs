using System;

namespace LdapAuthorizationService.Users.Internal
{
    public class ChangeStatusRequest
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
