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

// Serilog konfigürasyonu
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




// Veritabaný baðlantýsý için gerekli ekleme:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servislerin eklendiði satýrýn altýna ekleyin:
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Bu satýrýn mevcut olduðundan emin olun

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
 builder.Host.UseSerilog(...): Serilog'u uygulamaya ekler ve yapýlandýrýr.
ReadFrom.Configuration(context.Configuration): Serilog konfigürasyonunu appsettings.json dosyasýndan okur.
Enrich.FromLogContext(): Log mesajlarýna context bilgilerini (örneðin, request bilgilerini) ekler.
WriteTo.Console(): Log mesajlarýný konsola yazar.
WriteTo.Seq("http://localhost:5341"): Log mesajlarýný belirtilen adresteki Seq sunucusuna gönderir. Seq varsayýlan olarak 5341 portunu kullanýr.
app.UseSerilogRequestLogging(): Her HTTP request'i için otomatik olarak log kaydý oluþturur.
 */
