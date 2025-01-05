using GeminiEducationAPI.Domain.Entities;
using GeminiEducationAPI.Domain.Entities.Identity;
using GeminiEducationAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GeminiEducationAPI.Persistence.Data
{
    public static class SeedData
	{
		public static async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var services = scope.ServiceProvider;
				var loggerFactory = services.GetRequiredService<ILoggerFactory>();
				var context = services.GetRequiredService<ApplicationDbContext>(); // Context'i al
				var logger = loggerFactory.CreateLogger("SeedData"); // Logger'ı burada oluştur

				try
				{
					// Seed işleminin daha önce yapılıp yapılmadığını kontrol et
					var seedCompleted = await context.AppSettings.AnyAsync(s => s.Key == "SeedCompleted");
					if (seedCompleted)
					{
						logger.LogInformation("Seed data already exists. Skipping initialization.");
						return; // Veri varsa, seed işlemini atla
					}

					var userManager = services.GetRequiredService<UserManager<AppUser>>();
					var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

					// Rolleri oluştur
					if (!await roleManager.RoleExistsAsync("Admin"))
					{
						await roleManager.CreateAsync(new IdentityRole("Admin"));
					}
					if (!await roleManager.RoleExistsAsync("User"))
					{
						await roleManager.CreateAsync(new IdentityRole("User"));
					}

					// Admin kullanıcısını oluştur
					var adminUser = new AppUser { UserName = "admin", Email = "admin@example.com" };
					var result = await userManager.CreateAsync(adminUser, "Admin123!"); // Şifre: Admin123!
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}

					// Seed işleminin tamamlandığını belirten ayarı ekle
					context.AppSettings.Add(new AppSetting { Key = "SeedCompleted", Value = "true" });
					await context.SaveChangesAsync();


					logger.LogInformation("Seed data initialized successfully.");
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}
		}
	}
}
/*
 SeedData sınıfı static olarak tanımlanmıştır, çünkü bu sınıftan bir nesne oluşturmayacağız, sadece metotlarını kullanacağız.
InitializeAsync metodu, IServiceProvider'ı parametre olarak alır. Bu sayede, UserManager, RoleManager gibi gerekli servisleri dependency injection ile alabiliriz.
Geri kalan kod, Program.cs dosyasındaki kod ile aynıdır. Sadece, hata loglama için loggerFactory.CreateLogger<Program>(); yerine loggerFactory.CreateLogger<SeedData>(); kullanılmıştır.
=======================================================================================
ApplicationDbContext nesnesi, InitializeAsync metodu içerisinde alınıyor (var context = services.GetRequiredService<ApplicationDbContext>();).
context.AppSettings.AnyAsync(s => s.Key == "SeedCompleted") satırı, AppSettings tablosunda Key değeri SeedCompleted olan bir kayıt olup olmadığını kontrol eder.
Eğer böyle bir kayıt varsa, seed data işlemi daha önce yapılmış demektir ve InitializeAsync metodu return; ile erken bir şekilde sonlandırılır.
Seed data işlemi tamamlandıktan sonra, context.AppSettings.Add(new AppSetting { Key = "SeedCompleted", Value = "true" }); satırı ile AppSettings tablosuna SeedCompleted anahtarı ve true değeriyle yeni bir kayıt eklenir.
SeedCompleted adında bir log eklendi.
 */