using GeminiEducationAPI.Domain.Entities.Identity;
using GeminiEducationAPI.Domain.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace GeminiEducationAPI.Infrastructure.Token
{
	public class TokenGenerator : ITokenGenerator
	{
		private readonly ITokenOptions _tokenOptions;
		private readonly UserManager<AppUser> _userManager;

		public TokenGenerator(ITokenOptions tokenOptions, UserManager<AppUser> userManager)
		{
			_tokenOptions = tokenOptions;
			_userManager = userManager;
		}

		public string GenerateToken(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
			};

			// Kullanıcının rollerini al
			var roles = _userManager.GetRolesAsync(user).Result;

			// Her bir rolü claim olarak ekle
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

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
