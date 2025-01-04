using GeminiEducationAPI.Application.Features.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeminiEducationAPI.API.Controllers
{
	[Route("Account")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> Login([FromQuery] string? returnUrl, [FromBody] LoginUserCommand command)
		{
			var result = await _mediator.Send(command);

			if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}

			return Ok(result);
		}
	}
}
