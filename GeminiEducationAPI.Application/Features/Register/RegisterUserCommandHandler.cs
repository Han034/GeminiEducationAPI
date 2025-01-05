using GeminiEducationAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Register
{
	public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
	{
		private readonly UserManager<AppUser> _userManager;

		public RegisterUserCommandHandler(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var user = new AppUser
			{
				Email = request.Email,
				UserName = request.UserName
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				// Hata mesajlarını birleştir
				var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
				throw new System.Exception($"Kullanıcı oluşturulurken bir hata oluştu: {errorMessage}");
			}

			// Kullanıcıya "User" rolünü ata
			await _userManager.AddToRoleAsync(user, "User");
		}
	}
}
