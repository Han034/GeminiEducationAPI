using GeminiEducationAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using GeminiEducationAPI.Application;
using GeminiEducationAPI.Domain.Repositories;
using GeminiEducationAPI.Persistence.Repositories;
using GeminiEducationAPI.Application.Mappings;
using FluentValidation;
using GeminiEducationAPI.Persistence.Interceptors;
using GeminiEducationAPI.API.Middleware;
using Serilog;
using GeminiEducationAPI.API.Hubs;
using GeminiEducationAPI.Application.Interfaces;
using GeminiEducationAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using GeminiEducationAPI.Persistence.Data;
using GeminiEducationAPI.API.Extensions;
using GeminiEducationAPI.Infrastructure.Token;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Serilog konfig�rasyonu
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
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly); // Application.AssemblyReference -> AssemblyReference
builder.Services.AddScoped<AuditableEntityInterceptor>();
builder.Services.AddSignalR(); // SignalR'� ekle
builder.Services.AddScoped<IProductHubContext, ProductHubContext>();




// Veritaban� ba�lant�s� i�in gerekli ekleme:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();
	//builder.Services.AddIdentity<AppUser, IdentityRole>():1 Identity servislerini uygulamaya ekler.
	//AddEntityFrameworkStores<ApplicationDbContext>(): Entity Framework Core kullanarak Identity verilerini (kullan�c�lar, roller vb.) depolamak i�in gerekli servisleri ekler.
	//AddDefaultTokenProviders(): �ifre s�f�rlama gibi i�lemler i�in token �retmek i�in gerekli servisleri ekler.

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Middleware'i ekleyin.
app.UseSerilogRequestLogging(); // Request logging middleware'ini ekle
app.MapHub<ProductHub>("/productHub"); // SignalR Hub'�n� belirli bir URL'ye map'le

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Seed Data (Kullan�c� ve Rol Olu�turma)
using (var scope = app.Services.CreateScope())
{
	var serviceProvider = scope.ServiceProvider;
	await SeedData.InitializeAsync(serviceProvider);
}

app.UseCors(builder => builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
 builder.Host.UseSerilog(...): Serilog'u uygulamaya ekler ve yap�land�r�r.
ReadFrom.Configuration(context.Configuration): Serilog konfig�rasyonunu appsettings.json dosyas�ndan okur.
Enrich.FromLogContext(): Log mesajlar�na context bilgilerini (�rne�in, request bilgilerini) ekler.
WriteTo.Console(): Log mesajlar�n� konsola yazar.
WriteTo.Seq("http://localhost:5341"): Log mesajlar�n� belirtilen adresteki Seq sunucusuna g�nderir. Seq varsay�lan olarak 5341 portunu kullan�r.
app.UseSerilogRequestLogging(): Her HTTP request'i i�in otomatik olarak log kayd� olu�turur.
 */
