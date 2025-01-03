using GeminiEducationAPI.Domain.Entities;
using System.Linq.Expressions;

namespace GeminiEducationAPI.Domain.Repositories
{
	public interface IRepository<T> where T : BaseEntity
	{
		Task<T> GetByIdAsync(int id);
		Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
		IQueryable<T> GetAll();
		IQueryable<T> Find(Expression<Func<T, bool>> predicate);
		Task<T> AddAsync(T entity);
		Task AddRangeAsync(IEnumerable<T> entities);
		T Update(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entities);
	}
}
/*
 IRepository<T> where T : BaseEntity: Bu arayüz, generic bir yapıdır ve T tip parametresi alır. where T : BaseEntity kısıtlaması, T'nin yalnızca BaseEntity sınıfından türeyen sınıflar olabileceğini belirtir.
Task<T> GetByIdAsync(int id): Id'ye göre asenkron olarak bir entity getirir.
Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate): Verilen koşula (predicate) göre asenkron olarak tek bir entity getirir. Koşulu sağlaması beklenen tek bir kayıt olmalıdır. Aksi taktirde hata fırlatır.
IQueryable<T> GetAll(): Tüm entity'leri IQueryable olarak getirir. Bu sayede, sorgu veritabanında çalıştırılmadan önce üzerine ek filtrelemeler yapılabilir (örneğin, GetAll().Where(x => x.CreatedDate > DateTime.Now.AddDays(-7))).
IQueryable<T> Find(Expression<Func<T, bool>> predicate): Verilen koşula (predicate) uyan tüm entity'leri IQueryable olarak getirir.
Task<T> AddAsync(T entity): Asenkron olarak yeni bir entity ekler.
Task AddRangeAsync(IEnumerable<T> entities): Asenkron olarak bir entity listesi ekler.
T Update(T entity): Bir entity'i günceller.
void Remove(T entity): Bir entity'i siler.
void RemoveRange(IEnumerable<T> entities): Bir entity listesini siler.
 */