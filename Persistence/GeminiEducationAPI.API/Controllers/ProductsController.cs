using GeminiEducationAPI.Application.Features.Products.Commands.CreateProduct;
using GeminiEducationAPI.Application.Features.Products.Commands.DeleteProduct;
using GeminiEducationAPI.Application.Features.Products.Commands.UpdateProduct;
using GeminiEducationAPI.Application.Features.Products.Quaries.GetAllProducts;
using GeminiEducationAPI.Application.Features.Products.Quaries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeminiEducationAPI.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class ProductsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<ProductsController> _logger;

		public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(CreateProductCommand command)
		{
			_logger.LogInformation("CreateProduct metodu çağrıldı. Data: {@command}", command);

			var productId = await _mediator.Send(command);

			_logger.LogInformation("Product created successfully. ProductId: {ProductId}", productId);

			return Ok(productId);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var query = new GetAllProductsQuery();
			var products = await _mediator.Send(query);
			return Ok(products);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProductById(int id)
		{
			var query = new GetProductByIdQuery { Id = id };
			var product = await _mediator.Send(query);
			return Ok(product);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
		{
			await _mediator.Send(command);
			return Ok();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var command = new DeleteProductCommand { Id = id };
			await _mediator.Send(command);
			return Ok();
		}
	}
}
/*
 IMediator: MediatR arayüzü. Dependency injection ile constructor'da enjekte edilir.   
_mediator: IMediator nesnesine referans. Command ve query'leri göndermek için kullanılır.
---------------------------------------------------------------------------------------------
[HttpPost]: Bu action metodunun HTTP POST request'lerini karşılayacağını belirtir.   
CreateProduct(CreateProductCommand command): CreateProductCommand nesnesini parametre olarak alır. Bu nesne, request body'sinden otomatik olarak doldurulur.
await _mediator.Send(command): CreateProductCommand'i MediatR'a gönderir. MediatR, bu command'i uygun handler'a (CreateProductCommandHandler) yönlendirir.
return Ok(productId): CreateProductCommandHandler tarafından döndürülen productId değerini HTTP 200 (OK) status koduyla birlikte geri döndürür.
--------------------------------------------------------------------------------------------------
GetAllProducts: Bu action metodu, HTTP GET request'lerini karşılar ve tüm ürünleri listeler.
new GetAllProductsQuery(): Yeni bir GetAllProductsQuery nesnesi oluşturur.
await _mediator.Send(query): GetAllProductsQuery sorgusunu MediatR'a gönderir. MediatR, bu sorguyu GetAllProductsQueryHandler'a yönlendirir.
return Ok(products): GetAllProductsQueryHandler tarafından döndürülen GetAllProductsDto listesini HTTP 200 (OK) status koduyla birlikte geri döndürür.
GetProductById(int id): Bu action metodu, HTTP GET request'lerini karşılar ve belirli bir ürünü Id'sine göre getirir.
new GetProductByIdQuery { Id = id }: Yeni bir GetProductByIdQuery nesnesi oluşturur ve Id property'sini set eder.
await _mediator.Send(query): GetProductByIdQuery sorgusunu MediatR'a gönderir. MediatR, bu sorguyu GetProductByIdQueryHandler'a yönlendirir.
return Ok(product): GetProductByIdQueryHandler tarafından döndürülen GetProductByIdDto nesnesini HTTP 200 (OK) status koduyla birlikte geri döndürür.
--------------------------------------------------------------------------------------------------
UpdateProduct: Bu action metodu, HTTP PUT request'lerini karşılar ve bir ürünü günceller.
await _mediator.Send(command): UpdateProductCommand komutunu MediatR'a gönderir. MediatR, bu komutu UpdateProductCommandHandler'a yönlendirir.
return Ok(): İşlemin başarılı olduğunu belirtmek için HTTP 200 (OK) status kodu döndürür.
DeleteProduct(int id): Bu action metodu, HTTP DELETE request'lerini karşılar ve bir ürünü siler.
new DeleteProductCommand { Id = id }: Yeni bir DeleteProductCommand nesnesi oluşturur ve Id property'sini set eder.
await _mediator.Send(command): DeleteProductCommand komutunu MediatR'a gönderir. MediatR, bu komutu DeleteProductCommandHandler'a yönlendirir.
return Ok(): İşlemin başarılı olduğunu belirtmek için HTTP 200 (OK) status kodu döndürür.
=======================================================================================================
ILogger<ProductsController> _logger: ILogger arayüzünü dependency injection ile alıyoruz. ProductsController tipini belirttiğimiz için, log mesajlarında kaynak olarak ProductsController görünecektir.
_logger.LogInformation(...): LogInformation metodu ile bilgi seviyesinde log kaydı oluşturuyoruz.
"CreateProduct metodu çağrıldı. Data: {@command}": Log mesajı. @command ifadesi, command nesnesinin JSON formatında loglanmasını sağlar.
"Product created successfully. ProductId: {ProductId}": Log mesajı. {ProductId} ifadesi, productId değişkeninin değerinin loglanmasını sağlar.
 */
