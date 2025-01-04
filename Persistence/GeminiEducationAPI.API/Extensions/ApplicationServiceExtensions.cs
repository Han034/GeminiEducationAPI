using FluentValidation;
using GeminiEducationAPI.API.Hubs;
using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace GeminiEducationAPI.Application.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
			services.AddAutoMapper(typeof(MappingProfile));
			services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
			services.AddScoped<IProductHubContext, ProductHubContext>();

			return services;
		}
	}
}
