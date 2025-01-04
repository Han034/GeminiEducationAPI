using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Auth.Login
{
	public class LoginUserCommand : IRequest<string>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
