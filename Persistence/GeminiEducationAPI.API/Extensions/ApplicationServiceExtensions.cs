using FluentValidation;
using GeminiEducationAPI.API.Hubs;
using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiEducationAPI.Application.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
			services.AddScoped<IProductHubContext, ProductHubContext>();

			return services;
		}
	}
}
/*
 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme): Kimlik doğrulama servislerini uygulamaya ekler ve şema olarak JwtBearer'ı kullanacağını belirtir.
AddJwtBearer(options => ...): JWT doğrulama seçeneklerini yapılandırır.
TokenValidationParameters: Token'ın nasıl doğrulanacağını belirleyen parametreleri içerir.
ValidateIssuer = true: Token'daki Issuer alanının doğrulanmasını sağlar.
ValidateAudience = true: Token'daki Audience alanının doğrulanmasını sağlar.
ValidateLifetime = true: Token'ın geçerlilik süresinin (expiration) kontrol edilmesini sağlar.
ValidateIssuerSigningKey = true: Token'ı imzalamak için kullanılan anahtarın doğrulanmasını sağlar.
ValidIssuer = tokenOptions.Issuer: Beklenen Issuer değerini belirtir.
ValidAudience = tokenOptions.Audience: Beklenen Audience değerini belirtir.
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)): Token'ı imzalamak için kullanılacak gizli anahtarı belirtir.
app.UseAuthentication(): Kimlik doğrulama middleware'ini uygulamaya ekler. Bu satır, app.UseAuthorization() satırından önce gelmelidir.
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));: TokenOptions'u configuration'a bağlar.
builder.Services.AddSingleton<ITokenOptions>(sp => sp.GetRequiredService<IOptions<TokenOptions>>().Value);: ITokenOptions'u servis koleksiyonuna ekler.
 */
