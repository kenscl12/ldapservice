using LdapAuthorizationService.Auth.Internal;
using LdapAuthorizationService.Auth;
using LdapAuthorizationService.Ldap.Internal;
using LdapAuthorizationService.Ldap;
using LdapAuthorizationService.Repository.DbContext;
using LdapAuthorizationService.Users.Internal;
using LdapAuthorizationService.Users;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace LdapAuthorizationService.Extensions
{
	public static class StartupExtensions
	{
		public static void RegisterTypes(this IServiceCollection services)
		{
			services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
			services.AddScoped<ILdapService, LdapService>();
			services.AddScoped<ILdapUserManager, LdapUserManager>();
			services.AddScoped<ILdapGroupManager, LdapGroupManager>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
		}

		public static void AddConfigs(this IServiceCollection services, IConfiguration configuration)
		{
			var ldapSettings = configuration.GetSection(nameof(LdapSettings)).Get<LdapSettings>();
			services.AddSingleton(ldapSettings);

			var authentificationSettings = configuration.GetSection(nameof(AuthentificationSettings)).Get<AuthentificationSettings>();
			services.AddSingleton(authentificationSettings);
		}
		public static void AddAuthServiceDbContextAndMigrate(this IServiceCollection services,
			IConfiguration configuration, IWebHostEnvironment environment)
		{
			var connectionString = configuration.GetConnectionString("Postgre");
			services.AddDbContextPool<AuthServiceDbContext>(options => options
				.EnableSensitiveDataLogging(environment.IsDevelopment())
				.UseNpgsql(connectionString, serverOptions => serverOptions
					.CommandTimeout(120)
					.EnableRetryOnFailure(10)));

			var sp = services.BuildServiceProvider();
			using var scope = sp.CreateScope();
			var scopeServiceProvider = scope.ServiceProvider;
			var dbContext = scopeServiceProvider.GetRequiredService<AuthServiceDbContext>();
			dbContext.Database.Migrate();
			dbContext.Database.OpenConnection();
			((NpgsqlConnection)dbContext.Database.GetDbConnection()).ReloadTypes();
		}
	}
}
