using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Stores;

public class ProjectStore
{
  public Dictionary<string, Guid> Connections { get; set; } = [];
  public Dictionary<Guid, User> Users { get; set; } = [];
  public Dictionary<Guid, Game> Games { get; set; } = [];

  public User? TemporaryUser { get; set; }
  public Game? TemporaryGame { get; set; }
  public Game? FindGameContainingUser(Guid userId) => Games
    .FirstOrDefault(entry => entry.Value.UserIds.Contains(userId))
    .Value;
  public Game? FindGameByInviteCode(Guid inviteCode) => Games
    .FirstOrDefault(entry => entry.Value.InviteCode == inviteCode)
    .Value;
}
