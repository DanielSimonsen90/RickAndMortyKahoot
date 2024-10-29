using RickAndMortyKahoot.Models.Games;

namespace RickAndMortyKahoot.Models.Exceptions;

public class UserAlreadyConnectedException(Game game) : Exception
{
  public Game Game { get; set; } = game;
}
