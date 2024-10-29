using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Models.Exceptions.Games;
using RickAndMortyKahoot.Models.Exceptions.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  /// <summary>
  /// Create a game for <paramref name="userId"/>
  /// </summary>
  /// <param name="userId">User to be the host of the game</param>
  /// <param name="amountOfQuestions">Optional amount of questions to be used in the game. If null, all questions will be used</param>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="userId"/></exception>
  /// <exception cref="UserAlreadyOwnsGameException">If the user already owns a game</exception>
  public async Task CreateGame(Guid userId, int? amountOfQuestions = null) => await OnRecieveAction(Actions.CREATE_GAME, async () =>
  {
    // Get user and possible existing game
    var user = store.Users[userId] ?? throw new InvalidUserException();
    var game = store.FindGameContainingUser(userId);
    if (game is not null) throw new UserAlreadyOwnsGameException(game);

    // Create game and update references in store
    game = new Game(userId, questionService.GetGameQuestions(amountOfQuestions), amountOfQuestions);
    store.Games.Add(game.Id, game);
    
    user.GameId = game.Id;
    store.Users[userId] = user;
    
    // Add user to SignalR group
    AddConnectionToGroup(game.Id, userId);

    // Dispatch GAME_CREATE event
    await DispatchHubEvent(game.Id, Events.GAME_CREATE, game);
  });

  /// <summary>
  /// Start the game matching id <paramref name="gameId"/> if user with id <paramref name="userId"/> is the host
  /// </summary>
  /// <param name="gameId">Id of the game to start</param>
  /// <param name="userId">Id of the user that should be host of the game</param>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="userId"/></exception>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="gameId"/></exception>
  /// <exception cref="NotHostException">If the user is not the host of the game</exception>
  public async Task StartGame(Guid gameId, Guid userId) => await OnRecieveAction(Actions.START_GAME, async () =>
  {
    // Ensure user and game exists and user is host
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != userId) throw new NotHostException();

    // Update game state and references
    game.IsActive = true;
    game.Questions = questionService.GetGameQuestions(game.Limit);
    store.Games[gameId] = game;
    scoreService.DeleteScoresFromGame(game);

    // Dispatch GAME_START event
    await DispatchHubEvent(gameId, Events.GAME_START);
  });

  /// <summary>
  /// End the game matching id <paramref name="gameId"/> if user with id <paramref name="userId"/> is the host
  /// </summary>
  /// <param name="gameId">Id of the game to end</param>
  /// <param name="userId">Id of the user that should be host of the game</param>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="userId"/></exception>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="gameId"/></exception>
  /// <exception cref="NotHostException">If the user is not the host of the game
  public async Task Endgame(Guid gameId, Guid userId) => await OnRecieveAction(Actions.END_GAME, async () =>
  {
    // Ensure user and game exists and user is host
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != userId) throw new NotHostException();
    if (!game.IsActive) throw new InvalidGameStateException();

    // Update game state and references
    game.IsActive = false;
    store.Games[gameId] = game;

    // Get scores
    var scores = scoreService.GetHighscores(game).ToArray();

    // Dispatch GAME_END event
    await DispatchHubEvent(gameId, Events.GAME_END, scores);
  });
}
