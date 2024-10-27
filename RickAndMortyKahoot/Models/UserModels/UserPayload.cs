namespace RickAndMortyKahoot.Models.UserModels;

public class UserPayload(string username)
{
  public string Username { get; set; } = username;
}
