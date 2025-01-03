using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Commands.CreateProduct
{
	public class CreateProductCommand : IRequest<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		public int Stock { get; set; }
	}
}
