using Microsoft.EntityFrameworkCore;

namespace LdapAuthorizationService.Repository.DbContext
{
    public class AuthServiceDbContextFactory : DesignTimeDbContextFactoryBase<AuthServiceDbContext>
    {
        protected override AuthServiceDbContext CreateNewInstance(DbContextOptions<AuthServiceDbContext> options)
        {
            return new AuthServiceDbContext(options);
        }
    }
}