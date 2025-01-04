using GeminiEducationAPI.Domain;
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
				});

			services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
			services.AddSingleton<ITokenOptions>(sp => tokenOptions);

			return services;
		}
	}
}
/*
 JWT Nedir?

JWT, taraflar arasında bilgiyi güvenli bir şekilde aktarmak için kullanılan, açık standart (RFC 7519) haline gelmiş bir token formatıdır. Genellikle, kullanıcı kimlik doğrulaması ve yetkilendirme (authorization) için kullanılır.

JWT'nin Avantajları:

Stateless (Durumsuz): Token, kullanıcı hakkında gerekli tüm bilgileri içerir. Sunucunun, her request'te kullanıcının kimliğini doğrulamak için bir oturum bilgisi (session) tutmasına gerek kalmaz. Bu da uygulamayı daha ölçeklenebilir hale getirir.
Güvenli: Token, dijital olarak imzalanır ve şifrelenebilir. Bu sayede, token'ın değiştirilmesi veya sahte token üretilmesi engellenir.
Platformlar Arası Uyumlu: JWT, web, mobil ve masaüstü uygulamalarında kullanılabilir.
Yapacağımız İşlemler:

JWT oluşturmak ve doğrulamak için gerekli NuGet paketlerini yükleyeceğiz.
JWT ayarlarını yapılandıracağız (appsettings.json ve Program.cs).
Kullanıcı girişi (login) için bir API endpoint'i oluşturacağız.
Token oluşturma ve doğrulama işlemlerini gerçekleştireceğiz.
Protected endpoint'leri (korunan API endpoint'leri) JWT ile nasıl koruyacağımızı göreceğiz.
 */
