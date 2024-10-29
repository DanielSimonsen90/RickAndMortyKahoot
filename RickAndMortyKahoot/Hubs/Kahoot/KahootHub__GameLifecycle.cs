﻿using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  public async Task CreateGame(Guid userId, int? amountOfQuestions = null) => await OnRecieveAction(Actions.CREATE_GAME, async () =>
  {
    var user = store.Users[userId] ?? throw new InvalidUserException();
    var game = store.FindGameContainingUser(userId);
    if (game is not null) throw new UserAlreadyOwnsGameException(game);

    game = new Game(userId, questionService.GetGameQuestions(amountOfQuestions), amountOfQuestions);
    store.Games.Add(game.Id, game);
    
    user.GameId = game.Id;
    store.Users[userId] = user;
    
    AddConnectionToGroup(game.Id, userId);
    await DispatchHubEvent(game.Id, Events.GAME_CREATE, game);
  });

  public async Task StartGame(Guid gameId, Guid userId) => await OnRecieveAction(Actions.START_GAME, async () =>
  {
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();

    game.IsActive = true;
    game.Questions = questionService.GetGameQuestions(game.Limit);
    store.Games[gameId] = game;

    await DispatchHubEvent(gameId, Events.GAME_START);
  });

  public async Task Endgame(Guid gameId, Guid userId) => await OnRecieveAction(Actions.END_GAME, async () =>
  {
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != userId) throw new NotHostException();
    if (!game.IsActive) throw new InvalidGameException();

    game.IsActive = false;
    store.Games[gameId] = game;

    var scores = scoreService.GetHighscores(game).ToArray();
    scoreService.DeleteScoresFromGame(game);

    await DispatchHubEvent(gameId, Events.GAME_END, scores);
  });
}
