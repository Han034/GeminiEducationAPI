using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Features.Products.Commands.UpdateProduct
{
	public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
	{
		public UpdateProductCommandValidator()
		{
			RuleFor(p => p.Id).NotEmpty().WithMessage("{PropertyName} is required.");
			RuleFor(p => p.Name).NotEmpty().WithMessage("{PropertyName} is required.").Length(2, 150).WithMessage("{PropertyName} must be between 2 and 150 characters.");
			RuleFor(p => p.Description).NotEmpty().WithMessage("{PropertyName} is required.").Length(2, 500).WithMessage("{PropertyName} must be between 2 and 500 characters.");
			RuleFor(p => p.Price).NotEmpty().WithMessage("{PropertyName} is required.").GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
			RuleFor(p => p.Stock).NotEmpty().WithMessage("{PropertyName} is required.").GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
		}
	}
}
/*
 UpdateProductCommandValidator: Bu sınıf, UpdateProductCommand nesnesinin geçerliliğini kontrol eder.
AbstractValidator<UpdateProductCommand>: FluentValidation kütüphanesinden gelen ve UpdateProductCommand için doğrulama kuralları tanımlamamızı sağlayan base class.
RuleFor(): Doğrulama kuralı tanımlamak için kullanılır.
NotEmpty(): Alanın boş olmamasını sağlar.
Length(): Alanın minimum ve maksimum karakter uzunluğunu belirler.
GreaterThan(): Alanın değerinin belirli bir sayıdan büyük olmasını sağlar.
WithMessage(): Kural ihlal edildiğinde gösterilecek hata mesajını özelleştirir.
 */
