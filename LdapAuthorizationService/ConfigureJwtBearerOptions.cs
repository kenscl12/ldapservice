using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LdapAuthorizationService
{
	public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
	{
		private readonly IConfiguration _configuration;

		public ConfigureJwtBearerOptions(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void Configure(JwtBearerOptions options)
		{
			Configure(JwtBearerDefaults.AuthenticationScheme, options);
		}

		public void Configure(string name, JwtBearerOptions options)
		{
			if (name == JwtBearerDefaults.AuthenticationScheme)
			{
				var jwtTokenSecret = _configuration.GetValue<string>("AuthentificationSettings:jwtTokenSecret");

				options.TokenValidationParameters = new TokenValidationParameters();
				options.RequireHttpsMetadata = false;
				options.SaveToken = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = false,
					ValidateIssuer = false,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecret)),
					ClockSkew = TimeSpan.Zero
				};
			}
		}
	}
}
