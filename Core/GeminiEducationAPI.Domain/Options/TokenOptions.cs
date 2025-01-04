using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Domain.Options
{
	public class TokenOptions : ITokenOptions
	{
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public int AccessTokenExpiration { get; set; }
		public string SecurityKey { get; set; }
	}
}
/*
 
 Audience: Token'ın hangi hedef kitleye yönelik olduğunu belirtir (örneğin, web sitenizin adresi).
Issuer: Token'ı veren tarafı belirtir (örneğin, uygulamanızın adı veya adresi).
AccessTokenExpiration: Access token'ın geçerlilik süresini dakika cinsinden belirtir.
SecurityKey: Token'ı imzalamak için kullanılacak gizli anahtarı belirtir. Bu anahtar en az 16 karakter uzunluğunda olmalıdır ve üretim ortamında güvenli bir yerde saklanmalıdır.
 */
