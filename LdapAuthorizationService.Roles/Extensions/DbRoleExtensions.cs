using System.Collections.Generic;
using System.Linq;
using LdapAuthorizationService.Repository.Entities;
using LdapAuthorizationService.Users.Internal;

namespace LdapAuthorizationService.Users.Extensions
{
    public static class DbRoleExtensions
    {
        public static List<Role> ConvertToRoles(this List<DbRole> dbRoles)
        {
            if (dbRoles == null || !dbRoles.Any())
                return null;

            return dbRoles.Select(dbRole => dbRole.ConvertToRole()).ToList();
        }


        public static Role ConvertToRole(this DbRole dbRole)
        {
            var result = new Role()
            {
                Id = dbRole.Id,
                Name = dbRole.Name,
                CreatedDate = dbRole.CreatedDate,
                ModifiedDate = dbRole.ModifiedDate
            };

            return result;
        }
    }
}
