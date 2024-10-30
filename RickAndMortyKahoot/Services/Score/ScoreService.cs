using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Services.Question;
namespace RickAndMortyKahoot.Services.Score;

/// <summary>
/// Service to manage the scores of a <see cref="Game"/>
/// </summary>
/// <param name="questionService">Question service to check if an answer is correct</param>
public class ScoreService(QuestionService questionService)
{
  /// <summary>
  /// Global scores based on <see cref="User.Id"/> and score value
  /// [<see cref="User.Id"/>, score]
  /// </summary>
  public Dictionary<Guid, int> Scores { get; set; } = [];
  /// <summary>
  /// Scores that have been added to the current round, while the round is still ongoing. Based on <see cref="User.Id"/> and score value
  /// [<see cref="User.Id"/>, score]
  /// </summary>
  public Dictionary<Guid, int> NewScores { get; set; } = [];

  /// <summary>
  /// Calculate the score of an <see cref="Answer"/> based on the <see cref="Game"/> and <see cref="GameQuestion"/>
  /// </summary>
  /// <param name="game">Game that the answer is from</param>
  /// <param name="answer">Answer to calculate the score of</param>
  /// <param name="timedout">If the answer was submitted after the time ran out</param>
  /// <returns>Score of the answer</returns>
  public int CalculateScore(Game game, Answer answer, bool timedout)
  {
    // Find the question that the answer is for
    GameQuestion? question = game.Questions.Find(q => q.Id == answer.QuestionId);
    if (question is null) return 0;

    // Boolean values for if-statements
    bool isCorrectAnswer = questionService.IsCorrectAnswer(question, answer);
    bool isFastAnswer = IsFastAnswer(question, answer);

    // Calculate the score based on the answer, boolean values, timedout and ScoreModifiers
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
    .OrderByDescending(entry => entry.Value)
    .ToDictionary();

  /// <summary>
  /// Remove all user scores of <paramref name="game"/>
  /// </summary>
  /// <param name="game">Game to remove scores from</param>
  public void DeleteScoresFromGame(Game game)
  {
    foreach (Guid userId in game.UserIds)
    {
      Scores.Remove(userId);
    }
  }
  
  /// <summary>
  /// How fast a "fast answer" is. This is used to determine if a user answered the question quickly, adding a bonus to the score.
  /// </summary>
  private readonly TimeSpan FAST_ANSWER_THRESHOLD = TimeSpan.FromSeconds(20);

  /// <summary>
  /// Check if answer is "fast". This is used to determine if a user answered the question quickly, adding a bonus to the score.
  /// </summary>
  /// <param name="question">Question that the answer is for</param>
  /// <param name="answer">Answer to check if it is fast</param>
  /// <returns>If the answer is fast</returns>
  private bool IsFastAnswer(GameQuestion question, Answer answer)
  {
    DateTimeOffset sentAt = DateTimeOffset.FromUnixTimeMilliseconds(question.Timestamp);
    DateTimeOffset answeredAt = DateTimeOffset.FromUnixTimeMilliseconds(answer.Timestamp);
    TimeSpan answerTime = sentAt - answeredAt;

    return answerTime <= FAST_ANSWER_THRESHOLD;
  }

  /// <summary>
  /// Clears the cache of <see cref="Scores"/> & <see cref="NewScores"/>
  /// </summary>
  public void Clear()
  {
    Scores.Clear();
    NewScores.Clear();
  }
}
