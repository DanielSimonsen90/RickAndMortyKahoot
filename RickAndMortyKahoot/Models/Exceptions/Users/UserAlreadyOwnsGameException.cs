using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Models.Games;

namespace RickAndMortyKahoot.Models.Exceptions.Users;

/// <summary>
/// Exception thrown when <see cref="User"/> already owns a <see cref="Game"/>
/// </summary>
/// <param name="game">Game that the user already owns</param>
public class UserAlreadyOwnsGameException(Game game) : Exception
{
    public Game Game { get; } = game;
}
