using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
namespace RickAndMortyKahoot.Models.Games;

/// <summary>
/// Represents a game of "Kahoot"
/// </summary>
public class Game
{
  /// <summary>
  /// The default length of questions in a game to avoid lengthy games
  /// </summary>
  public const int DEFAULT_QUESTIONS_LENGTH = 10;

  /// <summary>
  /// DO NOT USE: Required for serialization
  /// </summary>
  public Game() : this(Guid.NewGuid(), Enumerable.Range(0, 10).Select(_ => new GameQuestion()).ToList()) { }
  /// <summary>
  /// Creates a new game with a host and a list of questions
  /// </summary>
  /// <param name="hostId">Id of the host (<see cref="User"/>)</param>
  /// <param name="questions">List of questions to add to the game</param>
  /// <param name="limit">Amount of questions to limit the game to</param>
  public Game(Guid hostId, List<GameQuestion> questions, int? limit = null)
  {
    HostId = hostId;
    UserIds = [hostId];
    Limit = limit ?? DEFAULT_QUESTIONS_LENGTH;
    Questions = questions;
  }

  public Guid Id { get; set; } = Guid.NewGuid();
  public Guid HostId { get; set; }
  public List<Guid> UserIds { get; set; }
  public Guid InviteCode = Guid.NewGuid();

  public List<GameQuestion> Questions { get; set; }
  public GameQuestion? CurrentQuestion { get; set; } = null;

  public bool IsActive { get; set; } = false;
  public int Limit { get; }
}
