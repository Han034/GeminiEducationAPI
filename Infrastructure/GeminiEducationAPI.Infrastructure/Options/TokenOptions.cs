﻿
namespace GeminiEducationAPI.Infrastructure.Options
{
	public class TokenOptions : ITokenOptions
	{
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public int AccessTokenExpiration { get; set; }
		public string SecurityKey { get; set; }
	}
}