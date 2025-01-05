using GeminiEducationAPI.Domain.Entities.Identity;
using GeminiEducationAPI.Infrastructure.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Auth.Login
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
	{
		private readonly ITokenGenerator _tokenGenerator;
		private readonly ILogger<LoginUserCommandHandler> _logger;

		public LoginUserCommandHandler(ITokenGenerator tokenGenerator, ILogger<LoginUserCommandHandler> logger)
		{
			_tokenGenerator = tokenGenerator;
			_logger = logger;
		}

		public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("LoginUserCommandHandler.Handle metodu çağrıldı. Kullanıcı: {Email}", request.Email);

			// Token üretiliyor, kullanıcı bilgisi, kontrolü vs. TokenGenerator içerisinde yapılacak.
			var token = await _tokenGenerator.GenerateToken(request.Email, request.Password);

			_logger.LogInformation("Token üretildi: {Email}", request.Email);
			return token;
		}
	}
}
