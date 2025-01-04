using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Domain.Entities
{
	public class AppUser : IdentityUser
	{
		// Ekstra özellikler buraya eklenebilir (Örn: Ad, Soyad, vb.)
	}
}
/*
 AppUser: Bu sınıf, IdentityUser sınıfından türetilerek oluşturulur ve uygulamamızdaki kullanıcıları temsil eder. IdentityUser sınıfı, Id, UserName, Email, PasswordHash gibi temel kullanıcı özelliklerini içerir.
 */
