using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Models.Games;

public class CreateGamePayload(Guid currentUserId, int amount) : IViewModel
{
  public int MaxAmountOfQuestions = amount;
  public int AmountOfQuestions { get; set; } = 10;

  public Guid? CurrentUserId => currentUserId;
}
