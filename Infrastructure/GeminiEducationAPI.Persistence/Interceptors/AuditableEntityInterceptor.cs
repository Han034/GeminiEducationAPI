using GeminiEducationAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GeminiEducationAPI.Persistence.Interceptors
{
	public class AuditableEntityInterceptor: SaveChangesInterceptor
	{
		public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavingChanges(eventData, result);
		}

		public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
		{
			UpdateAuditableEntities(eventData.Context);
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		private void UpdateAuditableEntities(DbContext? context)
		{
			if (context == null) return;

			foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
			{
				if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
				{
					entry.Entity.CreatedDate = DateTime.UtcNow;
				}

				if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
				{
					entry.Entity.UpdatedDate = DateTime.UtcNow;
				}
			}
		}
	}
}
/*
 Kod Açıklaması:

AuditableEntityInterceptor: SaveChangesInterceptor sınıfından türetilen bu sınıf, SaveChanges ve SaveChangesAsync metotları çağrıldığında araya girecek olan interceptor'ımızı tanımlar.
SavingChanges ve SavingChangesAsync: Bu metotlar, sırasıyla SaveChanges ve SaveChangesAsync metotlarından önce çağrılır.
UpdateAuditableEntities: Bu metot, eklenen veya güncellenen entity'lerin CreatedDate ve UpdatedDate özelliklerini doldurur.
context.ChangeTracker.Entries<BaseEntity>(): BaseEntity'den türeyen ve değiştirilmiş olan tüm entity'leri listeler.
entry.State == Microsoft.EntityFrameworkCore.EntityState.Added: Entity'nin yeni eklendiğini kontrol eder.
entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified: Entity'nin güncellendiğini kontrol eder.
 */