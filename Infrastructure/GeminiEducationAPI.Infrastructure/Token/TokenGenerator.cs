using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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
