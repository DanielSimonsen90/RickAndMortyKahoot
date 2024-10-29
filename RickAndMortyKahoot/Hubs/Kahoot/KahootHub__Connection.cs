using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Models.Exceptions.Games;
using RickAndMortyKahoot.Models.Exceptions.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  /// <summary>
  /// Connect user to game with <paramref name="inviteCode"/>
  /// </summary>
  /// <remarks>
  /// This action should match the <see cref="Actions.CONNECT"/> action
  /// </remarks>
  /// <param name="userId">Id of the user to connect to the game</param>
  /// <param name="inviteCode">InviteCode of the game to connec the user to</param>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="userId"/></exception>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="inviteCode"/></exception>
  /// <exception cref="UserAlreadyConnectedException">If the user is already connected to the game</exception>
  public async Task Connect(string userId, string inviteCode) => await OnRecieveAction(Actions.CONNECT, async () =>
  {
    // Get user and game
    User user = store.Users[Guid.Parse(userId)] ?? throw new InvalidUserException();
    Game game = store.FindGameByInviteCode(Guid.Parse(inviteCode)) ?? throw new InvalidGameException();
    // If user is already connected to the game, throw UserAlreadyConnectedException
    if (game.UserIds.Contains(Guid.Parse(userId))) throw new UserAlreadyConnectedException(game);

    // Update user and game references
    game.UserIds.Add(Guid.Parse(userId));
    store.Games[game.Id] = game;
    user.GameId = game.Id;

    // Add to SignalR group
    AddConnectionToGroup(game.Id, Guid.Parse(userId));

    // Dispatch USER_JOIN event
    await DispatchHubEvent(game.Id, Events.USER_JOIN, user);
  });

  /// <summary>
  /// Disconnect user from game
  /// </summary>
  /// <remarks>
  /// This action should match the <see cref="Actions.DISCONNECT"/> action
  /// </remarks>
  /// <param name="userId">The id of the user to disconnect</param>
  /// <param name="gameId">The id of the game to disconnect the user from</param>
  /// <exception cref="InvalidUserException">User not found by <paramref name="userId"/></exception>
  /// <exception cref="InvalidGameException">Game not found by <paramref name="gameId"/></exception>
  public async Task Disconnect(Guid userId, Guid gameId) => await OnRecieveAction(Actions.DISCONNECT, async () =>
  {
    // Get user and game
    User user = store.Users[userId] ?? throw new InvalidUserException();
    Game game = store.Games[gameId] ?? throw new InvalidGameException();
    if (!game.UserIds.Contains(userId)) return; // User already left

    // Update user and game references
    game.UserIds.Remove(userId);
    store.Games[gameId] = game;
    RemoveConnectionFromGroup(gameId);

    // Dispatch USER_LEAVE event
    await DispatchHubEvent(gameId, Events.USER_LEAVE, user);
  });

  /// <summary>
  /// Add <paramref name="userId"/> to <paramref name="gameId"/> SignalR group
  /// </summary>
  /// <param name="gameId">Id of the game</param>
  /// <param name="userId">Id of the user to add</param>
  private void AddConnectionToGroup(Guid gameId, Guid userId)
  {
    // SignalR group is more appropriate for this, however, due to MVC re-routing, client is disconnected and therefore cannot receive the group messages
    //await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());

    // Store connectionId to Connections in store
    store.Connections.Add(Context.ConnectionId, userId);
  }

  /// <summary>
  /// Remove connection from group
  /// </summary>
  /// <param name="gameId">Id of the game</param>
  private void RemoveConnectionFromGroup(Guid gameId)
  {
    // SignalR group is more appropriate for this, however, due to MVC re-routing and AddConnectionToGroup not being used properly, this is not needed
    //await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());

    // Remove connectionId from Connections in store
    store.Connections.Remove(Context.ConnectionId);
  }
}
