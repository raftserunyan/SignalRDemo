using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Models;

namespace SignalRDemo.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessage(Message message) => 
			await Clients.All.SendAsync("receiveMessage", message);
	}
}
