namespace RickAndMortyKahoot.Models.User;

public class User(string username)
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Username { get; } = username;

  public Guid? GameId { get; set; }
}
