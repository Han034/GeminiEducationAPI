
namespace GeminiEducationAPI.Domain.Entities
{
	public abstract class BaseEntity
	{
		public int Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedDate { get; set; }
	}
}
/*
 abstract anahtar kelimesi, bu sınıftan doğrudan bir nesne oluşturulamayacağını, sadece miras alınarak kullanılabileceğini belirtir.
Id: Her entity için benzersiz bir tanımlayıcı (Primary Key).
CreatedDate: Entity'nin oluşturulma tarihi. DateTime.UtcNow ile otomatik olarak atanır.
UpdatedDate: Entity'nin son güncellenme tarihi. Nullable (DateTime?) olarak tanımlanmıştır, yani başlangıçta bir değer atanmayabilir.
 */
