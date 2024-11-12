using LdapAuthorizationService.Extensions;
using LdapAuthorizationService.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LdapAuthorizationService
{
	public class Startup
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _environment;

		public Startup(IConfiguration configuration, IWebHostEnvironment environment)
		{
			_configuration = configuration;
			_environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson();
			services.AddSwaggerDocumentation($"{Program.ServiceName.Name}");

			services
				.AddHealthChecks();

			services.RegisterTypes();
			services.AddConfigs(_configuration);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer();

			services.ConfigureOptions<ConfigureJwtBearerOptions>();

			services.AddAuthServiceDbContextAndMigrate(_configuration, _environment);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors(builder => builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
			);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMiddleware<ApiExceptionMiddleware>();
			app.UseMiddleware<JwtMiddleware>();

			app.UseAuthentication();
			app.UseRouting();
			app.UseAuthorization();
			app.UseSwaggerDocumentation($"{Program.ServiceName.Name}");

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
