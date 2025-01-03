using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Commands.DeleteProduct
{
	public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
	{
		public DeleteProductCommandValidator()
		{
			RuleFor(p => p.Id).NotEmpty().WithMessage("{PropertyName} is required.");
		}
	}
}
/*
 DeleteProductCommandValidator: Bu sınıf, DeleteProductCommand nesnesinin geçerliliğini kontrol eder.
AbstractValidator<DeleteProductCommand>: FluentValidation kütüphanesinden gelen ve DeleteProductCommand için doğrulama kuralları tanımlamamızı sağlayan base class.
RuleFor(): Doğrulama kuralı tanımlamak için kullanılır.
NotEmpty(): Alanın boş olmamasını sağlar.
WithMessage(): Kural ihlal edildiğinde gösterilecek hata mesajını özelleştirir.
 */
