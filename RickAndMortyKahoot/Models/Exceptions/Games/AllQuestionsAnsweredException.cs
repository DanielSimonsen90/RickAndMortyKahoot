using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Games;
namespace RickAndMortyKahoot.Models.Exceptions.Games;

/// <summary>
/// Exception thrown when all <see cref="Question"/>s have been answered from <see cref="Game"/>.
/// </summary>
public class AllQuestionsAnsweredException : Exception
{
}
