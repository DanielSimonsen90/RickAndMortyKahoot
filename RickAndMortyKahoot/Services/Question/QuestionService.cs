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
        .Where(modelChoice => !modelChoice!.Equals(model))
        .Take(3)
        .Select(model => questionAroundProp(model)!.ToString()!)))
    .ToList();

  private static List<QuestionModel> DefineQuestionsFromPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, TProperty> questionAroundProp,
    Func<TProperty, TModel, string> question,
    Func<TProperty, bool>? filter = null,
    int take = QUESTION_AMOUNT_PER_TYPE
  ) => models
    .DistinctBy(questionAroundProp)
    .Where(model => filter is null || filter(questionAroundProp(model)))
    .Select((model, index, list) => new QuestionModel(
      title: question(questionAroundProp(model), model),
      answer: list.Count().ToString(),
      choices: new Random()
        .SelectRandomUniqueItems(list, 4)
        .Where(propChoice => !propChoice!.Equals(questionAroundProp(model)))
        .Take(3)
        .Select(prop => prop!.ToString()!)))
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
  #endregion
}
