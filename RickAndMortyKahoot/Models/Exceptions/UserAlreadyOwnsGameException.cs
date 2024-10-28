namespace RickAndMortyKahoot.Models.Exceptions;

public class UserAlreadyOwnsGameException(Game game) : Exception
{
  public Game Game { get; } = game;
}
