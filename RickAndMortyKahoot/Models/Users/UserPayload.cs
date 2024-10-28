namespace RickAndMortyKahoot.Models.Users;

public class UserPayload(string username)
{
  public string Username { get; set; } = username;
}
