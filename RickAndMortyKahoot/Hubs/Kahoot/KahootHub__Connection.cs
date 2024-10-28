using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Stores;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  public async Task Connect(string userId, string inviteCode) => await OnRecieveAction(Actions.CONNECT, async () =>
  {
    User user = store.Users[Guid.Parse(userId)] ?? throw new InvalidUserException();
    Game game = store.FindGameByInviteCode(Guid.Parse(inviteCode)) ?? throw new InvalidGameException();

    if (game.UserIds.Contains(Guid.Parse(userId))) throw new UserAlreadyConnectedException(game);

    game.UserIds.Add(Guid.Parse(userId));
    store.Games[game.Id] = game;
    user.GameId = game.Id;
    await AddConnectionToGroup(game.Id, Guid.Parse(userId));

    await DispatchHubEvent(game.Id, Events.USER_JOIN, user);
  });
  public async Task Disconnect(Guid userId, Guid gameId) => await OnRecieveAction(Actions.DISCONNECT, async () =>
  {
    User user = store.Users[userId] ?? throw new InvalidUserException();
    Game game = store.Games[gameId] ?? throw new InvalidGameException();

    if (!game.UserIds.Contains(userId)) return; // User already left

    game.UserIds.Remove(userId);
    store.Games[gameId] = game;
    await RemoveConnectionFromGroup(gameId);

    await DispatchHubEvent(gameId, Events.USER_LEAVE, user);
  });
  public override async Task OnConnectedAsync()
  {
    // Check if the connection exists in the store
    if (store.Connections.TryGetValue(Context.ConnectionId, out Guid userId)
        && store.Users.TryGetValue(userId, out User? user))
    {
      // Try to find the game for this user
      if (store.FindGameContainingUser(userId) is Game game)
      {
        game.UserIds.Add(userId);
        store.Games[game.Id] = game;
        await AddConnectionToGroup(game.Id, user.Id);
        await DispatchHubEvent(game.Id, Events.USER_JOIN, user);
      }
    }

    await base.OnConnectedAsync();
  }

  public override async Task OnDisconnectedAsync(Exception? exception)
  {
    if (store.Connections.TryGetValue(Context.ConnectionId, out Guid userId)
      && store.FindGameContainingUser(userId) is Game game
      && store.Users.TryGetValue(userId, out User? user))
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
