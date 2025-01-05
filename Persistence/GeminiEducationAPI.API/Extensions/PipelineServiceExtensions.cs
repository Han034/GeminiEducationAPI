using GeminiEducationAPI.API.Middleware;
using Serilog;

namespace GeminiEducationAPI.API.Extensions
{
	public static class PipelineServiceExtensions
	{
		public static void ConfigurePipeline(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeminiEducationAPI");
					c.RoutePrefix = string.Empty;
				});
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors(builder => builder
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());
			app.UseSerilogRequestLogging();
			app.UseMiddleware<ExceptionHandlingMiddleware>();
			app.UseAuthentication();
			app.UseAuthorization();
			app.Use(async (context, next) =>
			{
				await next();
				if (context.Response.StatusCode == 401)
				{
					var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
					logger.LogError("Unauthorized access attempted for endpoint: {endpoint}, User: {user}", context.Request.Path, context.User.Identity.Name);
				}
				else if (context.Response.StatusCode == 403)
				{
					var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
					logger.LogError("Forbidden access attempted for endpoint: {endpoint}, User: {user}", context.Request.Path, context.User.Identity.Name);
				}
			});
			app.MapControllers();
		}
	}
}
