using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Stores;

public class ProjectStore
{
  public List<User> Users { get; set; } = [];
  public List<Game> Games { get; set; } = [];
}
