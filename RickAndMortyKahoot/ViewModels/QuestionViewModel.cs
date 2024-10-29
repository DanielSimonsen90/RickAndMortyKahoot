using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

public class QuestionViewModel(GameQuestion question, int answered, int total)
{
  public GameQuestion Question { get; set; } = question;
  public int QuestionsAnswered { get; set; } = answered;
  public int QuestionsTotal { get; set; } = total;
}
