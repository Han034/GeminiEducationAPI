using GeminiEducationAPI.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProductPlugin
{
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
