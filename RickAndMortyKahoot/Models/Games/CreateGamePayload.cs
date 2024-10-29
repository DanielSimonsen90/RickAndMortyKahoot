using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Models.Games;

/// <summary>
/// Payload for creating a new game
/// </summary>
/// <param name="currentUserId">Host id</param>
/// <param name="amount">Amount of questions to add to the game</param>
public class CreateGamePayload(Guid currentUserId, int amount) : IViewModel
{
  public int MaxAmountOfQuestions = amount;
  public int AmountOfQuestions { get; set; } = 10;

  public Guid? CurrentUserId => currentUserId;
}
