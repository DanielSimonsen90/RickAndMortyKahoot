using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// ViewModel for the Game view in /Views/Game
/// </summary>
/// <param name="currentUserId">Current user's id - used to determine if the user is the game owner for dynamic rendering</param>
/// <param name="game">Game to be displayed</param>
/// <param name="score">Current game score</param>
public class GameViewModel(Guid currentUserId, Game game, Dictionary<Guid, int>? score = null) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public Game Game { get; set; } = game;

  public Dictionary<Guid, int> Score { get; set; } = score ?? [];
}
