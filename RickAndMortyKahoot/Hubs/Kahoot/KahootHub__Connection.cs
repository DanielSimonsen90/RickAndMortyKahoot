using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  public async Task Connect(Guid userId, Guid gameId)
  {
    User user = store.Users[userId] ?? throw new InvalidUserException();
    Game game = store.Games[gameId] ?? throw new InvalidGameException();

    if (game.UserIds.Contains(userId)) throw new UserAlreadyConnectedException(game);

    game.UserIds.Add(userId);
    store.Games[gameId] = game;
    await AddConnectionToGroup(gameId, userId);

    await DispatchHubEvent(gameId, Events.USER_JOIN, user);
  }
  public async Task Disconnect(Guid userId, Guid gameId)
  {
    User user = store.Users[userId] ?? throw new InvalidUserException();
    Game game = store.Games[gameId] ?? throw new InvalidGameException();

    if (!game.UserIds.Contains(userId)) return; // User already left

    game.UserIds.Remove(userId);
    store.Games[gameId] = game;
    await RemoveConnectionFromGroup(gameId);

    await DispatchHubEvent(gameId, Events.USER_LEAVE, user);
  }
  
  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    if (store.Connections[Context.ConnectionId] is Guid userId
      && store.FindGameContainingUser(userId) is Game game
      && store.Users[userId] is User user)
    {
      game.UserIds.Remove(userId);
      store.Games[game.Id] = game;
      await RemoveConnectionFromGroup(game.Id);
      await DispatchHubEvent(game.Id, Events.USER_LEAVE, user);
    }

    await base.OnDisconnectedAsync(exception);
  }

  private async Task AddConnectionToGroup(Guid gameId, Guid userId)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
    store.Connections.Add(Context.ConnectionId, userId);
  }
  private async Task RemoveConnectionFromGroup(Guid gameId)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
    store.Connections.Remove(Context.ConnectionId);
  }
}
