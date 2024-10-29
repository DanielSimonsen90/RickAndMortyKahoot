using RickAndMortyKahoot.Services.Score;
namespace RickAndMortyKahoot.Models.Questions;

/// <summary>
/// Represents an answer to a <see cref="Question"/>
/// </summary>
/// <param name="questionId">Id of the <see cref="Question"/> that the answer is for</param>
/// <param name="index">Index of the answer based on <see cref="Question.Choices"/></param>
/// <param name="userId">Id of the <see cref="User"/> that answered the question</param>
public class Answer(Guid questionId, int index, Guid? userId)
{
  public Guid QuestionId { get; } = questionId;
  /// <summary>
  /// Index of the answer based on <see cref="Question.Choices"/>
  /// </summary>
  public int Index { get; } = index;
  public Guid? UserId { get; } = userId;

  /// <summary>
  /// Timestamp of when the answer was submitted - used for calculating scores in <see cref="ScoreService"/>
  /// </summary>
  public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
