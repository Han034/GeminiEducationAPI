using GeminiEducationAPI.Domain.Repositories;
using GeminiEducationAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}
/*
 UnitOfWork: Bu sınıf, IUnitOfWork arayüzünü uygular.
_context: ApplicationDbContext nesnesine referans. Constructor'da enjekte edilir.
SaveChangesAsync(): Bu metot, ApplicationDbContext üzerindeki SaveChangesAsync() metodunu çağırarak değişiklikleri veritabanına kaydeder.
Dispose(): Bu metot, ApplicationDbContext nesnesini dispose ederek, yönetilmeyen kaynakları serbest bırakır.
 */
