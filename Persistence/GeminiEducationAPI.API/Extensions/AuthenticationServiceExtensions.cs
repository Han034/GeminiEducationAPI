using GeminiEducationAPI.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GeminiEducationAPI.API.Extensions
{
	public static class AuthenticationServiceExtensions
	{
		public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
		{
			var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = tokenOptions.Issuer,
						ValidAudience = tokenOptions.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
					};

					// Geri yönlendirme sorunlarını önlemek için:
					options.Events = new JwtBearerEvents
					{
						OnChallenge = context =>
						{
							// Redirect yerine doğrudan 401 döndür
							context.HandleResponse();
							context.Response.StatusCode = StatusCodes.Status401Unauthorized;
							return Task.CompletedTask;
						}
					};
				});

			services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
			services.AddSingleton<ITokenOptions>(sp => tokenOptions);

			return services;
		}
	}
}
