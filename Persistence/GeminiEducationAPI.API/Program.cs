using GeminiEducationAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using GeminiEducationAPI.Application;
using GeminiEducationAPI.Domain.Repositories;
using GeminiEducationAPI.Persistence.Repositories;
using GeminiEducationAPI.Application.Mappings;
using GeminiEducationAPI.Persistence.Interceptors;
using Serilog;
using Microsoft.AspNetCore.Identity;
using GeminiEducationAPI.Persistence.Data;
using GeminiEducationAPI.API.Extensions;
using GeminiEducationAPI.Infrastructure.Token;
using FluentValidation;
using GeminiEducationAPI.Domain.Entities.Identity;
using Microsoft.OpenApi.Models;
using GeminiEducationAPI.API.Hubs;
using System.Reflection;
using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Infrastructure.Files;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.


// Serilog konfigürasyonu
builder.Host.UseSerilog((context, loggerConfig) =>
{
	loggerConfig
		.ReadFrom.Configuration(context.Configuration)
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.WriteTo.Seq("http://localhost:5341"); // Seq sunucusunun adresi
});



builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
// File Service
builder.Services.AddScoped<IFileService, FileService>();


// Register the MediatR services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GeminiEducationAPI.Application.AssemblyReference).Assembly));


// Swagger Services
builder.Services.AddSwaggerServices(); 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddSwaggerGen(c =>
//{
//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
//	c.EnableAnnotations(); // Ek açýklamalarýný etkinleþtir

//	// XML Dokümantasyon dosyasýnýn yolunu belirtin
//	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//	c.IncludeXmlComments(xmlPath);
//});
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly); // Application.AssemblyReference -> AssemblyReference


builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddSignalR(); 

// Veritabaný baðlantýsý için gerekli ekleme:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();


// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();


//builder.Services.AddIdentity<AppUser, IdentityRole>():1 Identity servislerini uygulamaya ekler.
//AddEntityFrameworkStores<ApplicationDbContext>(): Entity Framework Core kullanarak Identity verilerini (kullanýcýlar, roller vb.) depolamak için gerekli servisleri ekler.
//AddDefaultTokenProviders(): Þifre sýfýrlama gibi iþlemler için token üretmek için gerekli servisleri ekler.

// Identity'nin cookie tabanlý doðrulama eklemesini devre dýþý býrakýn
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Events.OnRedirectToLogin = context =>
	{
		context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Yönlendirme yerine 401 döner
		return Task.CompletedTask;
	};

	options.Events.OnRedirectToAccessDenied = context =>
	{
		context.Response.StatusCode = StatusCodes.Status403Forbidden; // Yönlendirme yerine 403 döner
		return Task.CompletedTask;
	};
});

//var pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
//PluginLoader.LoadPlugins(builder.Services, pluginPath);
var pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
var pluginDirectory = Path.Combine(pluginPath, "Plugins");
PluginLoader.LoadPlugins(builder.Services, pluginDirectory);

var app = builder.Build();
app.ConfigurePipeline();

// Seed Data (Kullanýcý ve Rol Oluþturma)
using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	await SeedData.InitializeAsync(serviceProvider);
}

app.UseHttpsRedirection();

app.UseRouting(); // Önce Routing

app.UseCors(builder => builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseSerilogRequestLogging(); // Request logging middleware'ini ekle

app.UseAuthentication(); // Sonra Authentication
app.UseAuthorization(); // Sonra Authorization
app.MapHub<ProductHub>("/productHub");
app.MapControllers();

app.Run();

/*
 builder.Host.UseSerilog(...): Serilog'u uygulamaya ekler ve yapýlandýrýr.
ReadFrom.Configuration(context.Configuration): Serilog konfigürasyonunu appsettings.json dosyasýndan okur.
Enrich.FromLogContext(): Log mesajlarýna context bilgilerini (örneðin, request bilgilerini) ekler.
WriteTo.Console(): Log mesajlarýný konsola yazar.
WriteTo.Seq("http://localhost:5341"): Log mesajlarýný belirtilen adresteki Seq sunucusuna gönderir. Seq varsayýlan olarak 5341 portunu kullanýr.
app.UseSerilogRequestLogging(): Her HTTP request'i için otomatik olarak log kaydý oluþturur.
================================================================================================
using GeminiEducationAPI.API.Hubs; ifadesi eklendi.
builder.Services.AddSignalR(); satýrý eklendi. Bu satýr, SignalR servislerini dependency injection sistemine ekler.
app.MapHub<ProductHub>("/productHub"); satýrý eklendi. Bu satýr, ProductHub sýnýfýný /productHub URL'si ile eþleþtirir. Ýstemciler, bu URL'yi kullanarak Hub'a baðlanabilirler.
 */
