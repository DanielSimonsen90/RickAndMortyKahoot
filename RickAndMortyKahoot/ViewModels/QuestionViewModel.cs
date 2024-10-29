using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// View model for _Question PartialView in /Views/Game
/// </summary>
/// <param name="question">Question to display</param>
/// <param name="answered">Number of questions answered from the game</param>
/// <param name="total">Total number of questions in the game</param>
public class QuestionViewModel(GameQuestion question, int answered, int total)
{
  public GameQuestion Question { get; set; } = question;
  public int QuestionsAnswered { get; set; } = answered;
  public int QuestionsTotal { get; set; } = total;
}
