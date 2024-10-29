using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

public class CorrectAnswerViewModel(GameQuestion question, Answer answer, KeyValuePair<Guid, int>[] score) : ScoreListViewModel(score, false)
{
  public GameQuestion Question { get; } = question;
  public Answer Answer { get; } = answer;
}
