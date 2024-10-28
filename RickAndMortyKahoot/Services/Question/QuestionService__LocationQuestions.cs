using RickAndMorty.Net.Api.Models.Domain;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;
namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  private static List<QuestionModel> DefineLocationQuestions(IEnumerable<Location> locations)
  {
    // How many locations
    QuestionModel howManyLocations = DefineQuestions(locations,
      question: _ => "How many locations are there in Rick and Morty?",
      questionAroundProp: _ => locations.Count().ToString(),
      amountOfQuestions: 1).First();

    // How many types of locations?
    QuestionModel howManyTypes = DefineQuestionCountingPropertyValues(locations,
      question: "How many types of locations are there in Rick and Morty?",
      countDistinctProp: location => location.Type);

    // How many dimensions?
    QuestionModel howManyDimenstions = DefineQuestionCountingPropertyValues(locations,
      question: "How many dimensions are there in Rick and Morty?",
      countDistinctProp: location => location.Dimension);

    // How many residents are there in <name>
    List<QuestionModel> howManyResidentsInName = DefineQuestionsFromPropertyValues(locations,
      questionAroundProp: location => location.Residents.Count(),
      question: (count, location) => $"How many residents are there in {location.Name}?");

    // In what dimension is <name> in?
    List<QuestionModel> whatDimensionIsNameIn = DefineQuestions(locations,
      question: location => $"In what dimension is {location.Name} in?",
      questionAroundProp: location => location.Dimension);

    return
    [
      howManyLocations, howManyTypes, howManyDimenstions,
      .. howManyResidentsInName,
      .. whatDimensionIsNameIn
    ];
  }
}
