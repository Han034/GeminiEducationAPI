using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Interfaces
{
	public interface IProductHubContext
	{
		Task SendNewProductNotification(string message);
	}
}
