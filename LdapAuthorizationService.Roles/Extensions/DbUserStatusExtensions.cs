using System.Collections.Generic;
using System.Linq;
using LdapAuthorizationService.Repository.Entities;

namespace LdapAuthorizationService.Users.Extensions
{
    public static class DbUserStatusExtensions
    {
        public static List<CodeWithDescription> ConvertToUserStatuses(this List<DbUserStatus> dbUserStatuses)
        {
            if (dbUserStatuses == null || !dbUserStatuses.Any())
                return null;

            return dbUserStatuses.Select(dbUserStatus => dbUserStatus.ConvertToUserStatus()).ToList();
        }


        private static CodeWithDescription ConvertToUserStatus(this DbUserStatus dbUserStatus)
        {
            var result = new CodeWithDescription()
            {
                Code = dbUserStatus.Code,
                Description = dbUserStatus.Description
            };

            return result;
        }
    }
}
