namespace RickAndMortyKahoot.Models.QuestionModels;

public class GameQuestion(string title, string answer, string[] choices) : Question(title, answer, choices)
{
  public bool Available { get; set; }
  public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
