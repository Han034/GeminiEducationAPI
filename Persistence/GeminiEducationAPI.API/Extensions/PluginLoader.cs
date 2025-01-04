using System.Reflection;

namespace GeminiEducationAPI.API.Extensions
{
	public static class PluginLoader
	{
		public static void LoadPlugins(IServiceCollection services, string pluginPath)
		{
			if (!Directory.Exists(pluginPath))
			{
				Directory.CreateDirectory(pluginPath);
			}

			var pluginAssemblies = Directory.GetFiles(pluginPath, "*.dll")
				.Select(Assembly.LoadFrom)
				.ToList();

			foreach (var assembly in pluginAssemblies)
			{
				var types = assembly.GetTypes();
				var extensionMethod = types.FirstOrDefault(t => t.Name == "MyProductServiceExtensions"); // Eklenti içerisinde IServiceCollection'ı genişleten metodu bul
				if (extensionMethod != null)
				{
					var method = extensionMethod.GetMethod("AddMyProductService", BindingFlags.Static | BindingFlags.Public); // Genişletme metodunun adını kullan
					method?.Invoke(null, new object[] { services }); // Servisleri kaydet
				}
			}
		}
	}
}
