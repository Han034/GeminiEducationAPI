using AutoMapper;
using GeminiEducationAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeminiEducationAPI.Application.Features.Products.Quaries.GetAllProducts
{
	public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<GetAllProductsDto>>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<List<GetAllProductsDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
		{
			var products = await _productRepository.GetAll().AsNoTracking().ToListAsync(); // AsNoTracking() eklendi
			return _mapper.Map<List<GetAllProductsDto>>(products);
		}
	}
}
/*
 GetAllProductsQueryHandler: Bu sınıf, GetAllProductsQuery sorgusunu işleyen handler'dır.
IRequestHandler<GetAllProductsQuery, List<GetAllProductsDto>>: Bu handler'ın GetAllProductsQuery tipindeki sorguları işleyebileceğini ve List<GetAllProductsDto> tipinde bir sonuç döndüreceğini belirtir.
_productRepository: Product entity'si için repository arayüzü.
_mapper: Nesneler arası eşleme yapmak için kullanılan AutoMapper arayüzü.
Handle: Bu metot, GetAllProductsQuery sorgusu geldiğinde çalıştırılır. Repository'i kullanarak tüm ürünleri çeker, GetAllProductsDto listesine map'ler ve sonucu döndürür.
 */
