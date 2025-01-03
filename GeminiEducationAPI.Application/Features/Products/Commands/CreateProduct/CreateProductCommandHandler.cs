using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain.Repositories;
using MediatR;

namespace GeminiEducationAPI.Application.Features.Products.Commands.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork; // _unitOfWork değişkenini burada tanımlıyoruz.

		public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_productRepository = productRepository;
			_unitOfWork = unitOfWork; // Constructor'da enjekte edilen değeri değişkene atıyoruz.
		}

		public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = new Product
			{
				Name = request.Name,
				Description = request.Description,
				Price = request.Price,
				Stock = request.Stock
			};

			await _productRepository.AddAsync(product);
			await _unitOfWork.SaveChangesAsync(); // Artık _unitOfWork değişkenini kullanabiliriz.
			return product.Id;
		}
	}
}
