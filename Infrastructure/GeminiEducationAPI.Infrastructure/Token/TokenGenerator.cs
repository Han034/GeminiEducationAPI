using GeminiEducationAPI.Domain.Entities.Identity;
using GeminiEducationAPI.Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger<TokenGenerator> _logger;

		public TokenGenerator(ITokenOptions tokenOptions, UserManager<AppUser> userManager, ILogger<TokenGenerator> logger)
		{
			_tokenOptions = tokenOptions;
			_userManager = userManager;
			_logger = logger;
		}

		public async Task<string> GenerateToken(string email, string password)
		{
			_logger.LogInformation("GenerateToken metodu çağrıldı. Kullanıcı: {Email}", email);

			var user = await _userManager.FindByEmailAsync(email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, password))
			{
				_logger.LogError("Geçersiz kullanıcı adı veya şifre: {Email}", email);
				throw new Exception("Invalid credentials");
			}

			_logger.LogInformation("Kullanıcı doğrulandı: {Email}", email);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
			};

			// Kullanıcının rollerini al
			var roles = await _userManager.GetRolesAsync(user);

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
				expires: DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration),
				signingCredentials: creds
			);

			var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

			_logger.LogInformation("Token üretildi: {Email}, Token: {Token}", email, tokenString);

			return tokenString;
		}
	}
}
