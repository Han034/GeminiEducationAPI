using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace GeminiEducationAPI.Persistence.Contexts
{
	public class ApplicationDbContext : DbContext
	{
		private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntityInterceptor auditableEntityInterceptor) : base(options)
		{
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // Varsayılan davranışı NoTracking olarak ayarlar.
			_auditableEntityInterceptor = auditableEntityInterceptor;
			//			QueryTrackingBehavior.NoTracking: Varsayılan olarak hiçbir nesneyi izlemez.
			//			QueryTrackingBehavior.TrackAll: Varsayılan olarak tüm nesneleri izler.
		}

		// DbSet'ler buraya eklenecek (Örnek: public DbSet<Product> Products { get; set; } )
		public DbSet<Product> Products { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Fluent API konfigürasyonları buraya eklenecek
			modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("int");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
		}

	}
}
/*
 ApplicationDbContext : DbContext: Entity Framework Core'un DbContext sınıfından miras alır.
public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options): Constructor, DbContextOptions nesnesini alır ve base class'a iletir. Bu options nesnesi, veritabanı bağlantı dizesi gibi konfigürasyon bilgilerini içerir.
DbSet<T>: Her entity için DbSet property'leri burada tanımlanacak (Örneğin, public DbSet<Product> Products { get; set; }). Şimdilik boş.
OnModelCreating: Veritabanı modeli oluşturulurken Fluent API kullanarak ek konfigürasyonlar yapmak için bu metot override edilecek. Şimdilik boş.

=======================================================
_auditableEntityInterceptor: AuditableEntityInterceptor tipinde bir değişken tanımladık.
ApplicationDbContext constructor'ına AuditableEntityInterceptor tipinde bir parametre ekledik ve bu parametreyi _auditableEntityInterceptor değişkenine atadık.
OnConfiguring metodu override edildi ve optionsBuilder.AddInterceptors(_auditableEntityInterceptor) satırı eklendi. Bu satır, AuditableEntityInterceptor'ı DbContext'e ekler.
 */