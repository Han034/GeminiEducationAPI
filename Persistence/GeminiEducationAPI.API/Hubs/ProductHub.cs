using Microsoft.AspNetCore.SignalR;

namespace GeminiEducationAPI.API.Hubs
{
	public class ProductHub : Hub
	{
		public async Task SendNewProductNotification(string message)
		{
			await Clients.All.SendAsync("ReceiveNewProductNotification", message);
		}
	}
}
/*
 ProductHub: Hub sınıfından türetilen bu sınıf, SignalR Hub'ımızı temsil eder.
SendNewProductNotification: Bu metot, istemciler tarafından çağrılabilecek bir metottur. Şimdilik sadece bir mesaj gönderiyor.
Clients.All.SendAsync("ReceiveNewProductNotification", message): Tüm bağlı istemcilere ReceiveNewProductNotification adında bir mesaj gönderir. İstemciler, bu mesajı dinleyerek yeni ürün eklendiğinde haberdar olabilirler.
 */