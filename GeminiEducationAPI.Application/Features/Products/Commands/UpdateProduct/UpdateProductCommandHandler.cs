using GeminiEducationAPI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Commands.UpdateProduct
{
	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			if (product == null)
			{
				// Ürün bulunamazsa hata fırlatılabilir veya null döndürülebilir.
				throw new Exception("Product not found");
			}

			product.Name = request.Name;
			product.Description = request.Description;
			product.Price = request.Price;
			product.Stock = request.Stock;
			product.UpdatedDate = DateTime.UtcNow;

			_productRepository.Update(product);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
/*
 UpdateProductCommandHandler: Bu sınıf, UpdateProductCommand komutunu işleyen handler'dır.
IRequestHandler<UpdateProductCommand>: Bu handler'ın UpdateProductCommand tipindeki komutları işleyebileceğini ve herhangi bir değer döndürmeyeceğini belirtir.
_productRepository: Product entity'si için repository arayüzü.
_unitOfWork: Veritabanı işlemlerini bir transaction (işlem) içerisinde yönetmek için kullanılır.
Handle: Bu metot, UpdateProductCommand komutu geldiğinde çalıştırılır. Repository'i kullanarak ürünü Id'sine göre çeker, bilgileri günceller ve UnitOfWork kullanarak değişiklikleri veritabanına kaydeder.
 */
