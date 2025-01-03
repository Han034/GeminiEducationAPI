using GeminiEducationAPI.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Commands.DeleteProduct
{
	public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
	{
		private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByIdAsync(request.Id);

			if (product == null)
			{
				// Ürün bulunamazsa hata fırlatılabilir veya null döndürülebilir.
				throw new Exception("Product not found");
			}

			_productRepository.Remove(product);
			await _unitOfWork.SaveChangesAsync();
		}
	}
}
/*
 DeleteProductCommandHandler: Bu sınıf, DeleteProductCommand komutunu işleyen handler'dır.
IRequestHandler<DeleteProductCommand>: Bu handler'ın DeleteProductCommand tipindeki komutları işleyebileceğini ve herhangi bir değer döndürmeyeceğini belirtir.
_productRepository: Product entity'si için repository arayüzü.
_unitOfWork: Veritabanı işlemlerini bir transaction (işlem) içerisinde yönetmek için kullanılır.
Handle: Bu metot, DeleteProductCommand komutu geldiğinde çalıştırılır. Repository'i kullanarak ürünü Id'sine göre çeker, siler ve UnitOfWork kullanarak değişiklikleri veritabanına kaydeder.
 */
