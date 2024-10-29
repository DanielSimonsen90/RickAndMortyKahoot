namespace RickAndMortyKahoot.Models.Users;

/// <summary>
/// Represents a user in the system
/// </summary>
/// <param name="payload">Payload used to define the user</param>
public class User(UserPayload? payload = null)
{
  /// <summary>
  /// DO NOT USE: Used for serialization only
  /// </summary>
  public User() : this(null) { }

  public Guid Id { get; set; } = Guid.NewGuid();
  public string Username { get; set; } = payload?.Username ?? string.Empty;

  /// <summary>
  /// Id of the game the user is connected to, if any
  /// </summary>
  public Guid? GameId { get; set; }
}
