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
    AddConnectionToGroup(game.Id, Guid.Parse(userId));

    await DispatchHubEvent(game.Id, Events.USER_JOIN, user);
  });
  public async Task Disconnect(Guid userId, Guid gameId) => await OnRecieveAction(Actions.DISCONNECT, async () =>
  {
    User user = store.Users[userId] ?? throw new InvalidUserException();
    Game game = store.Games[gameId] ?? throw new InvalidGameException();

    if (!game.UserIds.Contains(userId)) return; // User already left

    game.UserIds.Remove(userId);
    store.Games[gameId] = game;
    RemoveConnectionFromGroup(gameId);

    await DispatchHubEvent(gameId, Events.USER_LEAVE, user);
  });

  private void AddConnectionToGroup(Guid gameId, Guid userId)
  {
    //await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
    store.Connections.Add(Context.ConnectionId, userId);
  }
  private void RemoveConnectionFromGroup(Guid gameId)
  {
    //await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
    store.Connections.Remove(Context.ConnectionId);
  }
}
