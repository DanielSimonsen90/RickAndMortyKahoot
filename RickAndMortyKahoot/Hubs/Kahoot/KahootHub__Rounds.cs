using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  public async Task NextQuestion(Guid gameId, Guid userId) => await OnRecieveAction(Actions.NEXT_QUESTION, async () =>
  {
    if (!store.Users.TryGetValue(userId, out User? user)) throw new InvalidUserException();
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != userId) throw new NotHostException();

    GameQuestion question = questionService.GetRandomGameQuestion(game);
    game.CurrentQuestion = question;
    store.Games[gameId] = game;

    await DispatchHubEvent(gameId, Events.NEW_QUESTION, question);
  });

  public async Task SubmitAnswer(Guid gameId, Answer answer) => await OnRecieveAction(Actions.SUBMIT_ANSWER, async () =>
  {
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (answer.UserId is null) throw new InvalidUserException();
    if (!store.Users.TryGetValue(answer.UserId.Value, out User? user)) throw new InvalidUserException();

    int score = scoreService.CalculateScore(game, answer, false);
    scoreService.NewScores.Add(answer.UserId.Value, score);

    bool newScoresContainsAllUsers = game.UserIds.All(userId => scoreService.NewScores.ContainsKey(userId));
    if (newScoresContainsAllUsers) await DispatchHubEvent(gameId, Events.AUTO_END_ROUND, game.HostId);
  });

  public async Task EndRound(Guid gameId, Guid hostId) => await OnRecieveAction(Actions.END_ROUND, async () =>
  {
    if (!store.Games.TryGetValue(gameId, out Game? game)) throw new InvalidGameException();
    if (game.HostId != hostId) throw new NotHostException();
    if (game.CurrentQuestion is null) throw new InvalidGameException();

    foreach (Guid userId in game.UserIds)
    {
      int newScore = scoreService.NewScores.TryGetValue(userId, out int value) 
        ? value 
        : scoreService.CalculateScore(game, new Answer(game.CurrentQuestion.Id, -1, userId), true);
      if (!scoreService.Scores.TryAdd(userId, newScore)) scoreService.Scores[userId] += newScore;
      scoreService.NewScores.Remove(userId);
    }

    var correctAnswer = new Answer(game.CurrentQuestion.Id, game.CurrentQuestion.AnswerIndex, null);
    var scores = game.UserIds.Aggregate(new Dictionary<Guid, int>(), (acc, userId) =>
    {
      acc.Add(userId, scoreService.Scores[userId]);
      return acc;
    });

    int index = game.Questions.IndexOf(game.CurrentQuestion);
    game.CurrentQuestion.Available = false;
    game.Questions[index] = game.CurrentQuestion;
    store.Games[gameId] = game;

    await DispatchHubEvent(gameId, Events.ROUND_END, correctAnswer, scores.ToArray(), game.CurrentQuestion);
  });
}
