using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain.Repositories;
using GeminiEducationAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GeminiEducationAPI.Persistence.Repositories
{
	public class Repository<T> : IRepository<T> where T : BaseEntity
	{
		private readonly ApplicationDbContext _context;
		protected readonly DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = _context.Set<T>();
		}

		public virtual async Task<T> AddAsync(T entity) // 'virtual' anahtar kelimesini ekledik
		{
			await _dbSet.AddAsync(entity);
			return entity;
		}

		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
		{
			return _dbSet.Where(predicate);
		}

		public IQueryable<T> GetAll()
		{
			return _dbSet;
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.SingleOrDefaultAsync(predicate);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			_dbSet.RemoveRange(entities);
		}

		public T Update(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
			return entity;
		}
	}
}
/*
 Repository<T> : IRepository<T> where T : BaseEntity: IRepository arayüzünü uygular ve T'nin BaseEntity'den türemesi gerektiğini belirtir.
_context: ApplicationDbContext nesnesine referans. Constructor'da enjekte edilecek.
_dbSet: Belirli bir T tipindeki entity'ler için DbSet nesnesine referans. _context.Set<T>() ile oluşturulur.
Metotlar, IRepository arayüzünde tanımlanan metotları Entity Framework Core metotlarını kullanarak uygular.
 */