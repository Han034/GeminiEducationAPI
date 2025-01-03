using GeminiEducationAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Domain.Repositories
{
	public interface IProductRepository : IRepository<Product>
	{
		// Product'a özel metotlar burada tanımlanabilir (eğer gerekirse)
	}
}
