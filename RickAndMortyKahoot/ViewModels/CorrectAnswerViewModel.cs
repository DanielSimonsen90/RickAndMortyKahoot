using RickAndMortyKahoot.Models.Questions;

namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// View model for the "_CorrectAnswer" partial view in /Views/Game
/// </summary>
/// <param name="question">Question asked</param>
/// <param name="answer">Correct answer to the question</param>
/// <param name="score">Current game score - passed to <see cref="ScoreListViewModel"/></param>
public class CorrectAnswerViewModel(GameQuestion question, Answer answer, KeyValuePair<Guid, int>[] score) : ScoreListViewModel(score, null)
{
  public GameQuestion Question { get; } = question;
  public Answer Answer { get; } = answer;
}
