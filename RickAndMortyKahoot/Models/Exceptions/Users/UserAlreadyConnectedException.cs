using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Models.Games;

namespace RickAndMortyKahoot.Models.Exceptions.Users;

/// <summary>
/// Exepction thrown when a <see cref="User"/> is already connected to a <see cref="Game"/>
/// </summary>
/// <param name="game">Game that the user is already connected to</param>
public class UserAlreadyConnectedException(Game game) : Exception
{
    public Game Game { get; set; } = game;
}
