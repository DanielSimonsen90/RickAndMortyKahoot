using RickAndMortyKahoot.Models.Questions;
namespace RickAndMortyKahoot.Models.Games;

public class Game
{
  public const int DEFAULT_QUESTIONS_LENGTH = 10;

  public Game() : this(Guid.NewGuid(), Enumerable.Range(0, 10).Select(_ => new GameQuestion()).ToList()) {}
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
  public Guid InviteCode = Guid.NewGuid();

  public List<GameQuestion> Questions { get; set; }

  public bool IsActive { get; set; } = false;
}
