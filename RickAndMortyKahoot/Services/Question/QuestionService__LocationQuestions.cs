using RickAndMorty.Net.Api.Models.Domain;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;
namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  /// <summary>
  /// Defines questions based on the locations data from RickAndMortyApi
  /// </summary>
  /// <param name="locations">Locations data from RickAndMortyApi</param>
  /// <returns>List of questions based on the locations data</returns>
  private static List<QuestionModel> DefineLocationQuestions(IEnumerable<Location> locations)
  {
    // How many locations
    QuestionModel howManyLocations = DefineQuestionsByListCount(locations,
      question: _ => "How many locations are there in Rick and Morty?",
      list: _ => locations,
      take: 1).First();

    // How many types of locations?
    QuestionModel howManyTypes = DefineQuestionCountingPropertyValues(locations,
      question: "How many types of locations are there in Rick and Morty?",
      countDistinctProp: location => location.Type);

    // How many dimensions?
    QuestionModel howManyDimenstions = DefineQuestionCountingPropertyValues(locations,
      question: "How many dimensions are there in Rick and Morty?",
      countDistinctProp: location => location.Dimension);

    // How many residents are there in <name>
    List<QuestionModel> howManyResidentsInName = DefineQuestionsByListCount(locations,
      question: location => $"How many residents are there in {location.Name}?",
      list: location => location.Residents);

    // In what dimension is <name> in?
    List<QuestionModel> whatDimensionIsNameIn = DefineQuestions(locations,
      question: location => $"In what dimension is \"{location.Name}\" in?",
      questionAroundProp: location => location.Dimension);

    return
    [
      howManyLocations, howManyTypes, howManyDimenstions,
      .. howManyResidentsInName,
      .. whatDimensionIsNameIn
    ];
  }
}
