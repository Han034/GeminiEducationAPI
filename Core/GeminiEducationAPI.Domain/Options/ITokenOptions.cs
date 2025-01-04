using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Domain.Options
{
	public interface ITokenOptions
	{
		string Audience { get; set; }
		string Issuer { get; set; }
		int AccessTokenExpiration { get; set; }
		string SecurityKey { get; set; }
	}
}
