using System.Text.Json.Serialization;

namespace RickAndMortyKahoot.Models.Users;

public class User(UserPayload? payload = null)
{
  public User() : this(null) { }

  public Guid Id { get; set; } = Guid.NewGuid();
  public string Username { get; set; } = payload?.Username ?? string.Empty;

  public Guid? GameId { get; set; }
}
