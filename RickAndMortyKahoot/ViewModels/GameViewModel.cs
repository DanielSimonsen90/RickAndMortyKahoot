using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

public class GameViewModel(Guid currentUserId, Game game, GameQuestion? currentQuestion = null, Dictionary<Guid, int>? score = null) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public Game Game { get; set; } = game;

  public GameQuestion? CurrentQuestion { get; set; } = currentQuestion;
  public Dictionary<Guid, int> Score { get; set; } = score ?? [];
}
