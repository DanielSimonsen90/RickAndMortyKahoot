namespace RickAndMortyKahoot.Models.Question;

public class Answer(Guid questionId, int index, Guid userId)
{
  public Guid QuestionId { get; } = questionId;
  public int Index { get; } = index;
  public Guid UserId { get; } = userId;
  public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
