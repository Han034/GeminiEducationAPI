using GeminiEducationAPI.Application.Features.Auth.Login;
using GeminiEducationAPI.Application.Features.Register;
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

		//[HttpPost("login")]
		//public async Task<IActionResult> Login([FromQuery] string? returnUrl, LoginUserCommand command)
		//{
		//	if (!ModelState.IsValid)
		//		return BadRequest("Invalid login request");

		//	try
		//	{
		//		var token = await _mediator.Send(command);

		//		if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
		//		{
		//			return Redirect(returnUrl);
		//		}

		//		return Ok(new
		//		{
		//			Token = token,
		//			Expiration = DateTime.UtcNow.AddMinutes(120)
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		return Unauthorized(new { error = ex.Message });
		//	}
		//}

		[HttpPost("[action]")]
		public async Task<IActionResult> Register(RegisterUserCommand command)
		{
			await _mediator.Send(command);
			return Ok();
		}
	}
}
