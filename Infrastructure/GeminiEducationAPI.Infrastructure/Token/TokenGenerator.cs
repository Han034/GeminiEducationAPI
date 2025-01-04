using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace GeminiEducationAPI.Infrastructure.Token
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly ITokenOptions _tokenOptions;

		public TokenGenerator(ITokenOptions tokenOptions)
		{
			_tokenOptions = tokenOptions;
		}

		public string GenerateToken(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
                // Diğer claim'ler buraya eklenebilir (örneğin, roller)
            };

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				_tokenOptions.Issuer,
				_tokenOptions.Audience,
				claims,
				expires: DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
/*
 ITokenGenerator: Bu interface, token üretmek için kullanılacak metodun imzasını tanımlar.
TokenGenerator: ITokenGenerator arayüzünü uygulayan bu sınıf, JWT token üretme işlemini gerçekleştirir.
_tokenOptions: ITokenOptions nesnesini dependency injection ile alır. Bu nesne, appsettings.json dosyasındaki token ayarlarını içerir.
GenerateToken(AppUser user): Bu metot, parametre olarak aldığı AppUser nesnesindeki bilgileri kullanarak bir JWT token oluşturur ve string olarak döndürür.
claims: Token'a eklenecek claim'leri (kullanıcı bilgileri) tanımlar. Bu örnekte, NameIdentifier (kullanıcı ID'si), Name (kullanıcı adı) ve Email claim'leri eklenmiştir.
key: Token'ı imzalamak için kullanılacak gizli anahtarı oluşturur.
creds: İmzalama bilgilerini (kimlik bilgileri) oluşturur.
token: JWT token nesnesini oluşturur.
Issuer: Token'ı veren taraf.
Audience: Token'ın hedef kitlesi.
claims: Token'a eklenecek claim'ler.
expires: Token'ın geçerlilik süresinin bitiş tarihi.
signingCredentials: İmzalama bilgileri.
new JwtSecurityTokenHandler().WriteToken(token): Token'ı string formatına çevirerek döndürür.
 */
