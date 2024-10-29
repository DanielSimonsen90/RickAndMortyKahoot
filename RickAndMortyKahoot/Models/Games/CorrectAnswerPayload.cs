namespace RickAndMortyKahoot.Models.Games;
#nullable disable

/// <summary>
/// Payload for getting the _CorrectAnswer PartialView
/// </summary>
public class CorrectAnswerPayload
{
  public int AnswerIndex { get; set; }
  public KeyValuePair<Guid, int>[] Score { get; set;  }
}