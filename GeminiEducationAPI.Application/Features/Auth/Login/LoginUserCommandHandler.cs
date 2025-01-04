using GeminiEducationAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using GeminiEducationAPI.Infrastructure.Token;


namespace GeminiEducationAPI.Application.Features.Auth.Login
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenGenerator _tokenGenerator;

		public LoginUserCommandHandler(UserManager<AppUser> userManager, ITokenGenerator tokenGenerator)
		{
			_userManager = userManager;
			_tokenGenerator = tokenGenerator;
		}

		public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
			{
				throw new Exception("Invalid credentials");
			}

			return _tokenGenerator.GenerateToken(user);
		}
	}
}
/*
 LoginUserCommand: Kullanıcı girişi için gerekli bilgileri (email ve şifre) içeren command sınıfı.
LoginUserCommandHandler: LoginUserCommand'i işleyen handler sınıfı.
_userManager: Kullanıcı işlemlerini yönetmek için UserManager servisi.
_signInManager: Oturum açma işlemlerini yönetmek için SignInManager servisi.
_tokenGenerator: Token üretmek için ITokenGenerator arayüzü.
Handle: Kullanıcı girişi işlemini gerçekleştirir.
_userManager.FindByEmailAsync: Email'e göre kullanıcıyı bulur.
_userManager.CheckPasswordAsync: Kullanıcının şifresini doğrular.
_tokenGenerator.GenerateToken: Kullanıcı için JWT token üretir.
 */