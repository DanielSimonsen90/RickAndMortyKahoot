using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Services.RickAndMortyApi;
using RickAndMortyKahoot.Utils;
using RickAndMortyKahoot.Extensions;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;

namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService(List<QuestionModel> questions)
{
  public List<QuestionModel> Questions { get; set; } = questions;
  
  public List<GameQuestion> GetGameQuestions(int? limit = null) => Questions
    .Take(limit ?? Questions.Count)
    .Select(question => new GameQuestion(
      title: question.Title,
      answer: question.Answer,
      choices: [.. question.Choices]))
    .ToList();
  public GameQuestion GetRandomGameQuestion(Game game)
  {
    if (game.Questions.All(q => !q.Available)) throw new AllQuestionsAnsweredException();

    List<GameQuestion> availableQuestions = game.Questions
      .Where(q => q.Available)
      .ToList();
    int index = new System.Random().Next(availableQuestions.Count);

    return availableQuestions[index];
  }
  public bool IsCorrectAnswer(QuestionModel question, Answer answer) => question.AnswerIndex == answer.Index;

  #region Define Questions
  public const int QUESTION_AMOUNT_PER_TYPE = 10;

  public static async Task<List<QuestionModel>> DefineAllQuestions(RickAndMortyKahootService kahootService)
  {
    var (characters, episodes, locations) = await kahootService.GetAllData();

    var characterQs = DefineCharacterQuestions(characters);
    var episodeQs= DefineEpisodeQuestions(episodes); 
    var locationQs = DefineLocationQuestions(locations);

    return 
    [
      .. DefineCharacterQuestions(characters),
      .. DefineEpisodeQuestions(episodes),
      .. DefineLocationQuestions(locations)
    ];
  }
  private static List<QuestionModel> DefineQuestions<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, string> question,
    Func<TModel, TProperty> questionAroundProp,
    Func<TProperty, bool>? filter = null,
    int amountOfQuestions = QUESTION_AMOUNT_PER_TYPE
  ) => new Random()
    .SelectRandomUniqueItems(models, amountOfQuestions)
    .Where(model => filter is null || filter(questionAroundProp(model)))
    .Select(model => new QuestionModel(
      title: question(model),
      answer: questionAroundProp(model)!.ToString()!,
      choices: models
        .Select(model => questionAroundProp(model)!.ToString()!)
        .Distinct()
        .Where(value => value != questionAroundProp(model)!.ToString())
        .Take(3)))
    .ToList();

  private static List<QuestionModel> DefineQuestionsFromPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, TProperty> questionAroundProp,
    Func<TProperty, TModel, string> question,
    Func<TProperty, bool>? filter = null,
    int take = QUESTION_AMOUNT_PER_TYPE
  ) => models
    .GroupBy(questionAroundProp)
    .Select(group => group
      .Where(model => filter is null || filter(questionAroundProp(model)))
      .Select((model, index, list) => new QuestionModel(
        title: question(questionAroundProp(model), model),
        answer: list.Count().ToString(),
        choices: new Random()
          .GetChoicesAround(list.Count())))
      .Take(1))
    .Aggregate(new List<QuestionModel>(), (acc, list) =>
    {
      acc.AddRange(list);
      return acc;
    })
    .Take(take)
    .ToList();

  private static QuestionModel DefineQuestionCountingPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    string question,
    Func<TModel, TProperty> countDistinctProp
  ) => new(
    title: question,
    answer: models.Select(countDistinctProp).Distinct().Count().ToString(),
    choices: new Random().GetChoicesAround(models.Select(countDistinctProp).Distinct().Count()));
  private static QuestionModel DefineQuestionCountingPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    string question,
    Func<TModel, bool> countFilter,
    Func<TModel, TProperty> distincyBy
  ) => new(
    title: question,
    answer: models.Where(countFilter).DistinctBy(distincyBy).Count().ToString(),
    choices: new Random().GetChoicesAround(models.Where(countFilter).Distinct().Count()));
  private static List<QuestionModel> DefineQuestionsByListCount<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, string> question,
    Func<TModel, IEnumerable<TProperty>> list,
    int take = QUESTION_AMOUNT_PER_TYPE
  ) => models
    .Select(model => new QuestionModel(
      title: question(model),
      answer: list(model).Count().ToString(),
      choices: new Random().GetChoicesAround(list(model).Count())))
    .Take(take)
    .ToList();
  #endregion
}
