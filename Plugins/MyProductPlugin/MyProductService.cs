using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace MyProductPlugin
{
	public class MyProductService : IProductService
	{
		public List<Product> GetProducts()
		{
			return new List<Product>
			{
				new Product { Id = 101, Name = "Plugin Product 1", Price = 10, Stock = 10, CreatedDate = DateTime.UtcNow },
				new Product { Id = 102, Name = "Plugin Product 2", Price = 20, Stock = 20, CreatedDate = DateTime.UtcNow }
			};
		}
		public string GetName()
		{
			return "MyProductPlugin";
		}
	}

	// Dependency Injection için genişletme metodu
	public static class MyProductServiceExtensions
	{
		public static IServiceCollection AddMyProductService(this IServiceCollection services)
		{
			services.AddTransient<IProductService, MyProductService>();
			return services;
		}
	}
}
