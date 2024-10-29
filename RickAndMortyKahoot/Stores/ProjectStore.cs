using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Hubs.Kahoot;

namespace RickAndMortyKahoot.Stores;

/// <summary>
/// Store for all data in the project
/// </summary>
public class ProjectStore
{
  /// <summary>
  /// Singleton instance of the store
  /// </summary>
  public static ProjectStore Instance { get; set; } = new ProjectStore();

  /// <summary>
  /// Private constructor to prevent instantiation
  /// </summary>
  private ProjectStore() { }

  /// <summary>
  /// All connections to the <see cref="KahootHub"/>
  /// </summary>
  public Dictionary<string, Guid> Connections { get; set; } = [];

  /// <summary>
  /// All users registered in the project
  /// </summary>
  public Dictionary<Guid, User> Users { get; set; } = [];

  /// <summary>
  /// All games created in the project
  /// </summary>
  public Dictionary<Guid, Game> Games { get; set; } = [];

  /// <summary>
  /// Find a <see cref="Game"/> that contains <see cref="User"/> matching the given <paramref name="userId"/> 
  /// </summary>
  /// <param name="userId">Id of the <see cref="User"/> to search for</param>
  /// <returns><see cref="Game"/> containing the <see cref="User"/> or null if not found</returns>
  public Game? FindGameContainingUser(Guid userId) => Games
    .FirstOrDefault(entry => entry.Value.UserIds.Contains(userId))
    .Value;

  /// <summary>
  /// Find a <see cref="Game"/> by the given <paramref name="inviteCode"/>
  /// </summary>
  /// <param name="inviteCode">Invite code of the <see cref="Game"/> to search for</param>
  /// <returns><see cref="Game"/> with the given <paramref name="inviteCode"/> or null if not found</returns>
  public Game? FindGameByInviteCode(Guid inviteCode) => Games
    .FirstOrDefault(entry => entry.Value.InviteCode == inviteCode)
    .Value;
}
