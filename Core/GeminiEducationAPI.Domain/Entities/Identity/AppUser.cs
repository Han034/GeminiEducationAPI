using Microsoft.AspNetCore.Identity;

namespace GeminiEducationAPI.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        // Ekstra özellikler buraya eklenebilir (Örn: Ad, Soyad, vb.)
    }
}
/*
 AppUser: Bu sınıf, IdentityUser sınıfından türetilerek oluşturulur ve uygulamamızdaki kullanıcıları temsil eder. IdentityUser sınıfı, Id, UserName, Email, PasswordHash gibi temel kullanıcı özelliklerini içerir.
 */
