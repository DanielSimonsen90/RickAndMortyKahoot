using Microsoft.AspNetCore.SignalR;
namespace RickAndMortyKahoot.Hubs;

public class KahootHub : Hub
{
  public async Task SendMessage(string user, string message)
  {
    await Clients.All.SendAsync("RecieveMessage", user, message);
  }
}
