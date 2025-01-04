using GeminiEducationAPI.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GeminiEducationAPI.API.Hubs
{
	public class ProductHubContext : IProductHubContext
	{
		private readonly IHubContext<ProductHub> _hubContext;

		public ProductHubContext(IHubContext<ProductHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public async Task SendNewProductNotification(string message)
		{
			await _hubContext.Clients.All.SendAsync("ReceiveNewProductNotification", message);
		}
	}
}
/*
 Bu sınıf, IProductHubContext arayüzünü uygular ve ProductHub'a erişmek için IHubContext<ProductHub>'ı kullanır.
 */
