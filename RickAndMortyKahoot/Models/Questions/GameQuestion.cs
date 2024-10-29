namespace RickAndMortyKahoot.Models.Questions;

/// <summary>
/// Represents a question in a game
/// </summary>
/// <param name="title">Question to ask</param>
/// <param name="answer">The correct answer</param>
/// <param name="choices">Other possible choices to select</param>
public class GameQuestion(string title, string answer, string[] choices) : Question(title, answer, choices)
{
  /// <summary>
  /// DO NOT USE - for serialization only
  /// </summary>
  public GameQuestion() : this(string.Empty, "answer", [string.Empty, string.Empty, string.Empty]) {}

  /// <summary>
  /// Whether the question is available to be answered
  /// </summary>
  public bool Available { get; set; } = true;

  /// <summary>
  /// Timestamp of when the question was asked
  /// </summary>
  public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
