using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Models.Games;
namespace RickAndMortyKahoot.Models.Exceptions.Games;

/// <summary>
/// Exception thrown when <see cref="User"/> is not the host of the <see cref="Game"/>
/// </summary>
public class NotHostException : Exception
{
}
