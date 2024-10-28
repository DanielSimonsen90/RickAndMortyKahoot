using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
namespace RickAndMortyKahoot.Models;

public class Game
{
  public const int DEFAULT_QUESTIONS_LENGTH = 10;

  public Game(Guid hostId, List<GameQuestion> questions)
  {
    HostId = hostId;
    UserIds = [hostId];

    if (questions.Count < DEFAULT_QUESTIONS_LENGTH) throw new IndexOutOfRangeException($"Questions must at least be of lengh {DEFAULT_QUESTIONS_LENGTH}");
    Questions = questions;
  }

  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid HostId { get; set; }
  public List<Guid> UserIds { get; set; }

  public List<GameQuestion> Questions { get; set; }

  public bool IsActive { get; set; } = false;
}
