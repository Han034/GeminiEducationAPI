using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Quaries.GetAllProducts
{
	public class GetAllProductsQuery : IRequest<List<GetAllProductsDto>>
	{
	}
}
