using GeminiEducationAPI.Application.Features.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiEducationAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login(LoginUserCommand command)
		{
			var result = await _mediator.Send(command);
			return Ok(result);
		}
	}
}
/*
 AuthController: Kimlik doğrulama (authentication) işlemlerini yönetmek için oluşturduğumuz controller sınıfı.
Login: Kullanıcı girişi için oluşturduğumuz action metodu. LoginUserCommand nesnesini parametre olarak alır ve geriye IActionResult döndürür.
_mediator.Send(command): LoginUserCommand'i MediatR'a gönderir
 */
