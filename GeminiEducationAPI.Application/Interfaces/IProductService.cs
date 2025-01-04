using GeminiEducationAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Interfaces
{
	public interface IProductService
	{
		List<Product> GetProducts();
		string GetName(); // Plugin'in adını döndürecek.

	}
}
