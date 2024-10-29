using RickAndMortyKahoot.Models.Exceptions.Users;
using RickAndMortyKahoot.Models.Exceptions.Games;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  /// <summary>
  /// Recieve the next question for the game with <paramref name="gameId"/> if user with <paramref name="userId"/> is the host
  /// </summary>
  /// <remarks>
  /// This action should match the <see cref="Actions.NEXT_QUESTION"/> action
  /// </remarks>
  /// <param name="gameId">Id of the game to recieve the next question for</param>
  /// <param name="userId">Id of the user that should be host of the game</param>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="userId"/></exception>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="gameId"/></exception>
  /// <exception cref="NotHostException">If the user is not the host of the game</exception>
  /// <exception cref="InvalidGameStateException">If the game is not active</exception>
  public async Task NextQuestion(Guid gameId, Guid userId) => await OnRecieveAction(Actions.NEXT_QUESTION, async () =>
  {
    // Ensure user and game exists and user is host
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != userId) throw new NotHostException();
    if (!game.IsActive) throw new InvalidGameStateException();

    try
    {
      // Get random question and update references
      GameQuestion question = questionService.GetRandomGameQuestion(game);
      question.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      game.CurrentQuestion = question;
      store.Games[gameId] = game;

      // Dispatch NEW_QUESTION event
      await DispatchHubEvent(gameId, Events.NEW_QUESTION, question);
    } 
    catch (AllQuestionsAnsweredException) // If questionService.GetRandomGameQuestion throws AllQuestionsAnsweredException
    {
      // Get the final score, delete scores from game and dispatch GAME_END event
      var scores = scoreService.GetHighscores(game);
      await DispatchHubEvent(gameId, Events.GAME_END, scores);
    }
  });

  /// <summary>
  /// Submit <paramref name="answer"/> for the game with <paramref name="gameId"/>
  /// </summary>
  /// <remarks>
  /// This action should match the <see cref="Actions.SUBMIT_ANSWER"/> action
  /// </remarks>
  /// <param name="gameId">Id of the game to submit the answer for</param>
  /// <param name="answer">Answer to submit</param>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="gameId"/></exception>
  /// <exception cref="InvalidUserException">If no user was found by <paramref name="answer.UserId"/></exception>
  /// <exception cref="InvalidGameStateException">If the game is not active or no current question</exception>
  public async Task SubmitAnswer(Guid gameId, Answer answer) => await OnRecieveAction(Actions.SUBMIT_ANSWER, async () =>
  {
    // Ensure game, user and question exists and game is active
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (answer.UserId is null) throw new InvalidUserException();
    if (!store.Users.TryGetValue(answer.UserId.Value, out User? user)) throw new InvalidUserException();
    if (!game.IsActive) throw new InvalidGameStateException();
    if (game.CurrentQuestion is null) throw new InvalidGameStateException();

    // Calculate score and add to NewScores until RoundEnd or AutoEndRound
    int score = scoreService.CalculateScore(game, answer, false);
    scoreService.NewScores.Add(answer.UserId.Value, score);

    // If all users have submitted their answers, dispatch AUTO_END_ROUND event
    bool newScoresContainsAllUsers = game.UserIds.All(userId => scoreService.NewScores.ContainsKey(userId));
    if (newScoresContainsAllUsers) await DispatchHubEvent(gameId, Events.AUTO_END_ROUND, game.HostId);
  });

  /// <summary>
  /// End the round for the game with <paramref name="gameId"/> if user with <paramref name="hostId"/> is the host
  /// </summary>
  /// <remarks>
  /// This action should match the <see cref="Actions.END_ROUND"/> action
  /// </remarks>
  /// <param name="gameId">Id of the game to end the round for</param>
  /// <param name="hostId">Id of the user that should be host of the game</param>
  /// <exception cref="InvalidGameException">If no game was found by <paramref name="gameId"/></exception>
  /// <exception cref="NotHostException">If the user is not the host of the game</exception>
  /// <exception cref="InvalidGameStateException">If no current question</exception>
  public async Task EndRound(Guid gameId, Guid hostId) => await OnRecieveAction(Actions.END_ROUND, async () =>
  {
    // Ensure game exists, user is host and game is active with current question
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != hostId) throw new NotHostException();
    if (!game.IsActive || game.CurrentQuestion is null) throw new InvalidGameStateException();

    // Calculate scores for all users and update references
    foreach (Guid userId in game.UserIds)
    {
      // Get the new score from scoreService.NewScores if user submitted an answer, else calculate score based on timedout answer
      int newScore = scoreService.NewScores.TryGetValue(userId, out int value) 
        ? value 
        : scoreService.CalculateScore(game, new Answer(game.CurrentQuestion.Id, -1, userId), true);

      // Add new score to user's current score in scoreService.Scores
      if (!scoreService.Scores.TryAdd(userId, newScore)) scoreService.Scores[userId] += newScore;

      // Remove user from NewScores to serve as temporary storage for the next round
      scoreService.NewScores.Remove(userId);
    }

    // Get correct answer and scores, then update references
    var correctAnswer = new Answer(game.CurrentQuestion.Id, game.CurrentQuestion.AnswerIndex, null);

    // Get scores for all users in the game
    var scores = scoreService.Scores
      .Where(pair => game.UserIds.Contains(pair.Key))
      .ToDictionary();

    // Update references
    int index = game.Questions.IndexOf(game.CurrentQuestion);
    game.CurrentQuestion.Available = false;
    game.Questions[index] = game.CurrentQuestion;
    store.Games[gameId] = game;

    // Dispatch ROUND_END event
    await DispatchHubEvent(gameId, Events.ROUND_END, correctAnswer, scores.ToArray(), game.CurrentQuestion);
  });
}
