using RickAndMortyKahoot.Models.Games;

namespace RickAndMortyKahoot.ViewModels;

public class GameViewModel(Guid currentUserId, Game game) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public Game Game { get; set; } = game;
}
