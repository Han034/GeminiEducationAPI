using GeminiEducationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeminiEducationAPI.Persistence.Contexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		// DbSet'ler buraya eklenecek (Örnek: public DbSet<Product> Products { get; set; } )

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Fluent API konfigürasyonları buraya eklenecek
		}
		public DbSet<Product> Products { get; set; }

	}
}
/*
 ApplicationDbContext : DbContext: Entity Framework Core'un DbContext sınıfından miras alır.
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options): Constructor, DbContextOptions nesnesini alır ve base class'a iletir. Bu options nesnesi, veritabanı bağlantı dizesi gibi konfigürasyon bilgilerini içerir.
DbSet<T>: Her entity için DbSet property'leri burada tanımlanacak (Örneğin, public DbSet<Product> Products { get; set; }). Şimdilik boş.
OnModelCreating: Veritabanı modeli oluşturulurken Fluent API kullanarak ek konfigürasyonlar yapmak için bu metot override edilecek. Şimdilik boş.
 */