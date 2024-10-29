namespace RickAndMortyKahoot.Models.Games;
#nullable disable

public class CorrectAnswerPayload
{
  public int AnswerIndex { get; set; }
  public KeyValuePair<Guid, int>[] Score { get; set;  }
}