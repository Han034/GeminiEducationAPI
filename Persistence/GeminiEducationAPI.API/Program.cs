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

// Register the MediatR services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GeminiEducationAPI.Application.AssemblyReference).Assembly));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly); // Application.AssemblyReference -> AssemblyReference
builder.Services.AddScoped<AuditableEntityInterceptor>();




// Veritaban� ba�lant�s� i�in gerekli ekleme:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servislerin eklendi�i sat�r�n alt�na ekleyin:
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Bu sat�r�n mevcut oldu�undan emin olun

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>(); // Middleware'i ekleyin.
app.UseSerilogRequestLogging(); // Request logging middleware'ini ekle


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
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
