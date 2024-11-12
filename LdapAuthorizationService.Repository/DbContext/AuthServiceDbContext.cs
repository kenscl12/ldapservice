using Microsoft.EntityFrameworkCore;
using LdapAuthorizationService.Repository.Entities;

namespace LdapAuthorizationService.Repository.DbContext
{
	public class AuthServiceDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbUserStatus> UserStatuses { get; set; }
        public DbSet<DbUser2Role> User2Roles { get; set; }
        public DbSet<DbRole> Roles { get; set; }
        public DbSet<DbOAuthRefreshToken> DbOAuthRefreshTokens { get; set; }

        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.HasDefaultSchema("default");

            mb.ApplyConfiguration(new DbUserConfiguration());
            mb.ApplyConfiguration(new RoleConfiguration());
            mb.ApplyConfiguration(new DbOAuthRefreshTokenConfiguration());
            mb.ApplyConfiguration(new DbUserStatusConfiguration());

            base.OnModelCreating(mb);
        }
    }
}
