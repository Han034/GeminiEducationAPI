using AutoMapper;
using GeminiEducationAPI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Quaries.GetProductById
{
	public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductByIdDto>
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;

		public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository;
			_mapper = mapper;
		}

		public async Task<GetProductByIdDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);
			return _mapper.Map<GetProductByIdDto>(product);
		}
	}
}
/*
 GetProductByIdQueryHandler: Bu sınıf, GetProductByIdQuery sorgusunu işleyen handler'dır.
IRequestHandler<GetProductByIdQuery, GetProductByIdDto>: Bu handler'ın GetProductByIdQuery tipindeki sorguları işleyebileceğini ve GetProductByIdDto tipinde bir sonuç döndüreceğini belirtir.
_productRepository: Product entity'si için repository arayüzü.
_mapper: Nesneler arası eşleme yapmak için kullanılan AutoMapper arayüzü.
Handle: Bu metot, GetProductByIdQuery sorgusu geldiğinde çalıştırılır. Repository'i kullanarak ürünü Id'sine göre çeker, GetProductByIdDto'ya map'ler ve sonucu döndürür.
 */
