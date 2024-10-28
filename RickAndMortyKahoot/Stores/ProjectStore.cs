using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Stores;

public class ProjectStore
{
  public Dictionary<string, Guid> Connections { get; set; } = [];
  public Dictionary<Guid, User> Users { get; set; } = [];
  public Dictionary<Guid, Game> Games { get; set; } = [];

  public User? CurrentUser { get; set; }
  public Game? CurrentGame { get; set; }
  public Game? FindGameContainingUser(Guid userId) => Games
    .FirstOrDefault(entry => entry.Value.UserIds.Contains(userId))
    .Value;
}
