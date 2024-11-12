using System.Collections.Generic;
using System.Linq;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Internal;
using LdapAuthorizationService.Users;

namespace LdapAuthorizationService.Users.Extensions
{
	public static class DbUserExtensions
	{
        public static List<User> ConvertToUsers(this List<DbUser> dbUsers)
        {
            if (dbUsers == null || !dbUsers.Any())
                return null;

            return dbUsers.Select(dbUser => dbUser.ConvertToUser()).ToList();
        }

        public static User ConvertToUser(this DbUser dbUser)
        {
            if (dbUser == null)
                return null;

            var result = new User
            {
                Id = dbUser.Id,
                CreatedDate = dbUser.CreatedDate,
                ExternalId = dbUser.ExternalId,
                Login = dbUser.Login,
                ModifiedDate = dbUser.ModifiedDate,
                Roles = dbUser.User2Roles.Select(t => new Role
                {
                    Id = t.Role.Id,
                    Name = t.Role.Name
                }).ToList(),
                UserType = (UserType)dbUser.UserType,
                Status = new CodeWithDescription {
                    Code = dbUser.Status,
                    Description = dbUser.UserStatus?.Description
                }
            };

            return result;
        }
    }
}
