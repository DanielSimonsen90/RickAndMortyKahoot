using RickAndMortyKahoot.Models.Games;
namespace RickAndMortyKahoot.Models.Exceptions.Games;

/// <summary>
/// Exception thrown when a <see cref="Game"/> is in an invalid state
/// </summary>
/// <remarks>
/// Reasons could be <see cref="Game.IsActive"/> is false or <see cref="Game.CurrentQuestion"/> is null
/// </remarks>
public class InvalidGameStateException : Exception
{
}
