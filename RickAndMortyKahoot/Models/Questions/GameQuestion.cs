namespace RickAndMortyKahoot.Models.Questions;

public class GameQuestion(string title, string answer, string[] choices) : Question(title, answer, choices)
{
  public GameQuestion() : this(string.Empty, "answer", [string.Empty, string.Empty, string.Empty]) {}

  public bool Available { get; set; }
  public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
