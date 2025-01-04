using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Infrastructure.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
