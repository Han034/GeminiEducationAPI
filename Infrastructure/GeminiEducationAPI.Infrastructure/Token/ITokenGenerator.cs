using GeminiEducationAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Infrastructure.Token
{
    public interface ITokenGenerator
	{
		string GenerateToken(AppUser user);
	}
}
