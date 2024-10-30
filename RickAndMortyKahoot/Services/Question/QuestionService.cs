using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Services.RickAndMortyApi;
using RickAndMortyKahoot.Extensions;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;
using RickAndMortyKahoot.Models.Exceptions.Games;

namespace RickAndMortyKahoot.Services.Question;

/// <summary>
/// Service for handling <see cref="GameQuestion"/>s and defining <see cref="QuestionModel"/>s
/// </summary>
/// <param name="questions">List of all <see cref="QuestionModel"/>s</param>
public partial class QuestionService(List<QuestionModel> questions)
{
  /// <summary>
  /// List of all <see cref="QuestionModel"/>s
  /// </summary>
  public List<QuestionModel> Questions { get; set; } = questions;
  
  /// <summary>
  /// Get a list of <see cref="GameQuestion"/>s
  /// </summary>
  /// <param name="limit">Limit the result using <see cref="IEnumerable{T}.Take(int)"/></param>
  public List<GameQuestion> GetGameQuestions(int? limit = null) => Questions
    .OrderRandomly()
    .Take(limit ?? Questions.Count) // If no limit is provided, take all questions
    .Select(question => new GameQuestion(
      title: question.Title,
      answer: question.Answer,
      choices: [.. question.Choices]))
    .ToList();

  /// <summary>
  /// Get a random <see cref="GameQuestion"/> from a <see cref="Game"/>
  /// </summary>
  /// <param name="game">Game to get the question from</param>
  /// <returns>Random, available, <see cref="GameQuestion"/></returns>
  /// <exception cref="AllQuestionsAnsweredException">Thrown when all questions have been answered</exception>
  public GameQuestion GetRandomGameQuestion(Game game)
  {
    // Check if all questions have been answered
    if (game.Questions.All(q => !q.Available)) throw new AllQuestionsAnsweredException();

    // From the available questions, get a random question
    List<GameQuestion> availableQuestions = game.Questions
      .Where(q => q.Available)
      .ToList();
    int index = new Random().Next(availableQuestions.Count);
    GameQuestion question = availableQuestions[index];

    // Randomize the choices and return the question
    question.Choices = [.. question.Choices.OrderRandomly()];
    return question;
  }

  /// <summary>
  /// Check if an <see cref="Answer"/> is the correct answer to a <see cref="QuestionModel"/>
  /// </summary>
  /// <param name="question">The question the <see cref="answer"/> is to</param>
  /// <param name="answer">The answer to the <see cref="question"/></param>
  /// <returns>If the <see cref="answer"/> is correct</returns>
  public bool IsCorrectAnswer(QuestionModel question, Answer answer) => question.AnswerIndex == answer.Index;
  
  /// <summary>
  /// Clears the cache of <see cref="Question"/>
  /// </summary>
  public void Clear()
  {
    Questions.Clear();
  }

  #region Define Questions
  /// <summary>
  /// Don't take more than <value> amount of questions per type of question
  /// </summary>
  public const int QUESTION_AMOUNT_PER_TYPE = 10;

  /// <summary>
  /// Define all questions based on the data from the <see cref="RickAndMortyKahootService"/>
  /// </summary>
  /// <param name="kahootService">Service to get the data from</param>
  /// <returns>List of all <see cref="QuestionModel"/>s</returns>
  public static async Task<List<QuestionModel>> DefineAllQuestions(RickAndMortyKahootService kahootService)
  {
    // Get all data from the service
    var (characters, episodes, locations) = await kahootService.GetAllData();

    // Define questions for each type of data
    var characterQs = DefineCharacterQuestions(characters);
    var episodeQs= DefineEpisodeQuestions(episodes); 
    var locationQs = DefineLocationQuestions(locations);

    // Combine all questions and return
    List<QuestionModel> result =
    [
      .. DefineCharacterQuestions(characters),
      .. DefineEpisodeQuestions(episodes),
      .. DefineLocationQuestions(locations)
    ];

    // In occasion, choices or answers return empty strings - filter those away
    return result
      .Where(q => q.Choices.All(choice => choice != string.Empty) || q.Answer != string.Empty)
      .ToList();
  }

  /// <summary>
  /// Custom definition of a question
  /// </summary>
  /// <typeparam name="TModel">Model used to define the questions</typeparam>
  /// <typeparam name="TProperty">Property to base the question around</typeparam>
  /// <param name="models">List of models to define questions from</param>
  /// <param name="question">Function to define the question</param>
  /// <param name="questionAroundProp">Function to get the property to base the question around</param>
  /// <param name="filter">Optional filter to apply to the property</param>
  /// <param name="amountOfQuestions">Optional amount of questions to take. Default is <see cref="QUESTION_AMOUNT_PER_TYPE"/></param>
  /// <returns>List of <see cref="QuestionModel"/>s created</returns>
  private static List<QuestionModel> DefineQuestions<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, string> question,
    Func<TModel, TProperty> questionAroundProp,
    Func<TProperty, bool>? filter = null,
    int amountOfQuestions = QUESTION_AMOUNT_PER_TYPE
  ) => models
    .Where(model => filter is null || filter(questionAroundProp(model)))
    .Select(model => new QuestionModel(
      title: question(model),
      answer: questionAroundProp(model)!.ToString()!,
      choices: models
        .Select(model => questionAroundProp(model)!.ToString()!)
        .Distinct()
        .Where(value => value != questionAroundProp(model)!.ToString())
        .OrderRandomly()
        .Take(3)))
    .OrderRandomly()
    .Take(amountOfQuestions)
    .ToList();

  /// <summary>
  /// Define questions based on matching property values
  /// </summary>
  /// <typeparam name="TModel">Model used to define the questions</typeparam>
  /// <typeparam name="TProperty">Property to base the question around</typeparam>
  /// <param name="models">List of models to define questions from</param>
  /// <param name="questionAroundProp">Function to get the property to base the question around</param>
  /// <param name="question">Function to define the question</param>
  /// <param name="filter">Optional filter to apply to the property</param>
  /// <param name="take">Optional amount of questions to take. Default is <see cref="QUESTION_AMOUNT_PER_TYPE"/></param>
  /// <returns>List of <see cref="QuestionModel"/>s created</returns>
  private static List<QuestionModel> DefineQuestionsFromPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    Func<TModel, TProperty> questionAroundProp,
    Func<TProperty, TModel, string> question,
    Func<TProperty, bool>? filter = null,
    int take = QUESTION_AMOUNT_PER_TYPE
  ) => models
    .GroupBy(questionAroundProp) // Group by the property to get unique values
    .Select(group => group
      .Where(model => filter is null || filter(questionAroundProp(model)))
      .Select((model, index, list) => new QuestionModel(
        title: question(questionAroundProp(model), model),
        answer: list.Count().ToString(), // Answer is the amount of models with the same property value
        choices: new Random().GetChoicesAround(list.Count())))
      .Take(1))
    // Combine the groups to a single list of questions
    .Aggregate(new List<QuestionModel>(), (acc, list) =>
    {
      acc.AddRange(list);
      return acc;
    })
    .Take(take)
    .ToList();

  /// <summary>
  /// Define questions based on counting property values
  /// </summary>
  /// <typeparam name="TModel">Model used to define the questions</typeparam>
  /// <typeparam name="TProperty">Property to base the question around</typeparam>
  /// <param name="models">List of models to define questions from</param>
  /// <param name="question">Function to define the question</param>
  /// <param name="countDistinctProp">Function to get the property to count distinct values</param>
  /// <returns>List of <see cref="QuestionModel"/>s created</returns>
  private static QuestionModel DefineQuestionCountingPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    string question,
    Func<TModel, TProperty> countDistinctProp
  ) => new(
    title: question,
    answer: models.Select(countDistinctProp).Distinct().Count().ToString(),
    choices: new Random().GetChoicesAround(models.Select(countDistinctProp).Distinct().Count()));
  
  /// <summary>
  /// Define questions based on counting property values
  /// </summary>
  /// <typeparam name="TModel">Model used to define the questions</typeparam>
  /// <typeparam name="TProperty">Property to base the question around</typeparam>
  /// <param name="models">List of models to define questions from</param>
  /// <param name="question">Function to define the question</param>
  /// <param name="countFilter">Filter to apply to the property</param>
  /// <param name="distincyBy">Function to get the property to count distinct values</param>
  /// <returns>List of <see cref="QuestionModel"/>s created</returns>
  private static QuestionModel DefineQuestionCountingPropertyValues<TModel, TProperty>(
    IEnumerable<TModel> models,
    string question,
    Func<TModel, bool> countFilter,
    Func<TModel, TProperty> distincyBy
  ) => new(
    title: question,
    answer: models.Where(countFilter).DistinctBy(distincyBy).Count().ToString(),
    choices: new Random().GetChoicesAround(models.Where(countFilter).Distinct().Count()));

  /// <summary>
  /// Define questions based on provided list's count
  /// </summary>
  /// <typeparam name="TModel">Model used to define the questions</typeparam>
  /// <typeparam name="TProperty">Property to base the question around</typeparam>
  /// <param name="models">List of models to define questions from</param>
  /// <param name="question">Function to define the question</param>
  /// <param name="list">Function to get the list to count</param>
  /// <param name="take">Optional amount of questions to take. Default is <see cref="QUESTION_AMOUNT_PER_TYPE"/></param>
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
    .OrderRandomly()
    .Take(take)
    .ToList();
  #endregion
}
