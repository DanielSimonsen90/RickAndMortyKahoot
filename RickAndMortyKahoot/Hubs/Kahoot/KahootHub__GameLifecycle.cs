using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Exceptions;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  public async Task CreateGame(Guid userId, int? amountOfQuestions = null) => await OnRecieveAction(Actions.CREATE_GAME, async () =>
  {
    var game = store.FindGameContainingUser(userId);
    if (game is not null) throw new UserAlreadyOwnsGameException(game);

    game = new Game(userId, questionService.GetGameQuestions(amountOfQuestions));
    store.Games.Add(game.Id, game);
    await AddConnectionToGroup(game.Id, userId);
    await DispatchHubEvent(game.Id, Events.GAME_CREATE, game);
  });
}
