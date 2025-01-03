using GeminiEducationAPI.Domain.Entities;
using System.Linq.Expressions;
using GeminiEducationAPI.Persistence.Repositories;
using GeminiEducationAPI.Persistence.Contexts;

namespace GeminiEducationAPI.Domain.Repositories
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		public ProductRepository(ApplicationDbContext context) : base(context)
		{
		}

		// AddAsync metodunun implementasyonu:
		public override async Task<Product> AddAsync(Product entity)
		{
			await _dbSet.AddAsync(entity);
			return entity;
		}
	}
}
