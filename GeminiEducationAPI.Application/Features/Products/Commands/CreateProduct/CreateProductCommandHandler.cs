using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain.Repositories;
using MediatR;

namespace GeminiEducationAPI.Application.Features.Products.Commands.CreateProduct
{
	public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductHubContext _productHubContext;

		public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IProductHubContext productHubContext)
		{
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
			_productHubContext = productHubContext;
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
			await _unitOfWork.SaveChangesAsync();
			await _productHubContext.SendNewProductNotification($"New product added with Id: {product.Id}");

			return product.Id;
		}
	}
}

/*
 IHubContext<ProductHub> _hubContext değişkenine eklendi ve constructor'da initialize edildi. Bu, ProductHub'a erişmemizi ve istemcilere mesaj göndermemizi sağlar.
CreateProduct metodu güncellendi: _hubContext.Clients.All.SendAsync("ReceiveNewProductNotification", $"New product added with Id: {productId}"); satırı eklendi. Bu satır, yeni ürün eklendikten sonra tüm istemcilere ReceiveNewProductNotification mesajını gönderir.
 */