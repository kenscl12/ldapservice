using Microsoft.OpenApi.Models;

namespace LdapAuthorizationService.Extensions
{
	public static class SwaggerExtensions
	{
		private const string _swaggerApplicationVersion = "v1";

		/// <summary>
		/// Добавление службы
		/// </summary>
		/// <param name="services"></param>
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, string appName)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(
					_swaggerApplicationVersion,
					new OpenApiInfo
					{
						Version = _swaggerApplicationVersion,
						Title = appName,
						Description = $"Api for information by {appName}"
					});


				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});

				// add config file for xml comments
				//var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				//var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
				//c.IncludeXmlComments(filePath);

				c.CustomSchemaIds(type => type.ToString());
			});

			return services;
		}


		/// <summary>
		/// Middleware для подключения настроек сваггера
		/// </summary>
		/// <param name="app"></param>
		/// <param name="appName"></param>
		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, string appName)
		{
			app.UseSwagger();
			app.UseSwaggerUI(
					c =>
					{
						c.SwaggerEndpoint($"/swagger/{_swaggerApplicationVersion}/swagger.json", $"{appName} {_swaggerApplicationVersion}");
					});

			return app;
		}
	}
}
