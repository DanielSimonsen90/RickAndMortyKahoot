using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

public class CorrectAnswerViewModel(GameQuestion question, Answer answer, KeyValuePair<Guid, int>[] score)
{
  public GameQuestion Question { get; } = question;
  public Answer Answer { get; } = answer;
  public KeyValuePair<Guid, int>[] Score { get; } = score;

  public string FormatScore(int score) => score.ToString("N0").Replace(',', '.');
}
