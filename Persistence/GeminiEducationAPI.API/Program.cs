using GeminiEducationAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using GeminiEducationAPI.Application;
using GeminiEducationAPI.Domain.Repositories;
using GeminiEducationAPI.Persistence.Repositories;
using GeminiEducationAPI.Application.Mappings;
using GeminiEducationAPI.Persistence.Interceptors;
using Serilog;
using GeminiEducationAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using GeminiEducationAPI.Persistence.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using GeminiEducationAPI.API.Extensions;
using GeminiEducationAPI.Infrastructure.Token;
using FluentValidation;
using GeminiEducationAPI.API.Extensions;


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

// Authentication Services
builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();

// Register the MediatR services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GeminiEducationAPI.Application.AssemblyReference).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

// Veritabaný baðlantýsý için gerekli ekleme:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
	//builder.Services.AddIdentity<AppUser, IdentityRole>():1 Identity servislerini uygulamaya ekler.
	//AddEntityFrameworkStores<ApplicationDbContext>(): Entity Framework Core kullanarak Identity verilerini (kullanýcýlar, roller vb.) depolamak için gerekli servisleri ekler.
	//AddDefaultTokenProviders(): Þifre sýfýrlama gibi iþlemler için token üretmek için gerekli servisleri ekler.

// Swagger Services
builder.Services.AddSwaggerServices();

var pluginPath = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
PluginLoader.LoadPlugins(builder.Services, pluginPath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

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

app.MapControllers();

app.Run();

/*
 builder.Host.UseSerilog(...): Serilog'u uygulamaya ekler ve yapýlandýrýr.
ReadFrom.Configuration(context.Configuration): Serilog konfigürasyonunu appsettings.json dosyasýndan okur.
Enrich.FromLogContext(): Log mesajlarýna context bilgilerini (örneðin, request bilgilerini) ekler.
WriteTo.Console(): Log mesajlarýný konsola yazar.
WriteTo.Seq("http://localhost:5341"): Log mesajlarýný belirtilen adresteki Seq sunucusuna gönderir. Seq varsayýlan olarak 5341 portunu kullanýr.
app.UseSerilogRequestLogging(): Her HTTP request'i için otomatik olarak log kaydý oluþturur.
 */
