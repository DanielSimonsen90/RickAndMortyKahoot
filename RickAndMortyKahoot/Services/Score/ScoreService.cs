﻿using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Services.Question;
using System.Linq;
namespace RickAndMortyKahoot.Services.Score;

public class ScoreService(QuestionService questionService)
{
  /// <summary>
  /// [<see cref="User.Id"/>, score]
  /// </summary>
  public Dictionary<Guid, int> Scores { get; set; } = [];
  /// <summary>
  /// [<see cref="User.Id"/>, score]
  /// </summary>
  public Dictionary<Guid, int> NewScores { get; set; } = [];

  public int CalculateScore(Game game, Answer answer, bool timedout)
  {
    GameQuestion? question = game.Questions.Find(q => q.Id == answer.QuestionId);
    if (question is null) return 0;

    bool isCorrectAnswer = questionService.IsCorrectAnswer(question, answer);
    bool isFastAnswer = IsFastAnswer(question, answer);

    int score = 0;
    if (isCorrectAnswer) score += ScoreModifier.CORRECT_ANSWER;
    else score += ScoreModifier.WRONG_ANSWER;
    if (isFastAnswer && isCorrectAnswer) score += ScoreModifier.FAST_ANSWER;
    else if (!timedout) score += ScoreModifier.SUBMITTED_ANSWER;
    else score += ScoreModifier.TIMEDOUT_ANSWER; 
    return score;
  }

  /// <summary>
  /// Get the game's highscores ordered by score
  /// </summary>
  /// <param name="game"></param>
  /// <returns>[<see cref="User.Id"/>, score] ordered by highest</returns>
  /// <exception cref="NotImplementedException"></exception>
  public Dictionary<Guid, int> GetHighscores(Game game) =>  Scores
    .Where(entry => game.UserIds.Contains(entry.Key))
    .OrderBy(entry => entry.Value)
    .ToDictionary();

  public void DeleteScoresFromGame(Game game)
  {
    foreach (Guid userId in game.UserIds)
    {
      Scores.Remove(userId);
    }
  }
  
  private readonly TimeSpan FAST_ANSWER_THRESHOLD = TimeSpan.FromSeconds(20);
  private bool IsFastAnswer(GameQuestion question, Answer answer)
  {
    DateTimeOffset sentAt = DateTimeOffset.FromUnixTimeMilliseconds(question.Timestamp);
    DateTimeOffset answeredAt = DateTimeOffset.FromUnixTimeMilliseconds(answer.Timestamp);
    TimeSpan answerTime = sentAt - answeredAt;

    return answerTime <= FAST_ANSWER_THRESHOLD;
  }
}
