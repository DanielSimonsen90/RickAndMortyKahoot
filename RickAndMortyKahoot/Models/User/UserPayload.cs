namespace RickAndMortyKahoot.Models.User;

public class UserPayload(string username)
{
  public string Username { get; set; } = username;
}
