using GeminiEducationAPI.API.Models;
using System.Net;
using System.Text.Json;

namespace GeminiEducationAPI.API.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			var response = context.Response;

			var errorResponse = new ErrorResponse
			{
				StatusCode = response.StatusCode,
				Message = "Internal Server Error." // Varsayılan bir mesaj
			};

			// Burada sadece log'a yazıyoruz, özel bir durum (örneğin, ApplicationException) kontrolü yok.
			response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var result = JsonSerializer.Serialize(errorResponse);
			await context.Response.WriteAsync(result);
		}
	}
}
/*
 ExceptionHandlingMiddleware: IMiddleware arayüzünden türetilen bu sınıf, HTTP request pipeline'ında araya girerek hataları yakalayacak olan middleware'imizi tanımlar.
_next: Sonraki middleware'i temsil eder.
InvokeAsync: Bu metot, her HTTP request'i için çağrılır.
try...catch: _next(httpContext) çağrısını try...catch bloğu içine alarak, bu middleware'den sonraki middleware'lerde oluşabilecek hataları yakalar.
HandleExceptionAsync: Hata oluştuğunda bu metot çağrılır.
HandleExceptionAsync: Bu metot, hatayı işler ve bir ErrorResponse nesnesi oluşturarak HTTP yanıtı olarak geri döndürür.
context.Response.ContentType = "application/json": Yanıtın tipini JSON olarak ayarlar.
errorResponse: ErrorResponse nesnesini oluşturur.
switch (exception): Farklı hata tiplerine göre farklı işlemler yapmamızı sağlar.
ApplicationException: Uygulamaya özel hatalar için kullanılır.
default: Diğer tüm hatalar için HttpStatusCode.InternalServerError (500) döndürülür.
JsonSerializer.Serialize(errorResponse): ErrorResponse nesnesini JSON'a çevirir.
context.Response.WriteAsync(result): JSON'ı HTTP yanıtı olarak yazar.
 */
