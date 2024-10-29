using RickAndMorty.Net.Api.Models.Domain;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;

namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  /// <summary>
  /// Define questions based on the characters data from RickAndMortyApi
  /// </summary>
  /// <param name="characters">Characters data from RickAndMortyApi</param>
  /// <returns>List of questions based on the characters data</returns>
  private static List<QuestionModel> DefineCharacterQuestions(IEnumerable<Character> characters)
  {
    // How many characters
    QuestionModel howManyCharacters = DefineQuestionsByListCount(characters,
      question: _ => "How many characters are there in Rick and  Morty?",
      list: _ => characters,
      take: 1).First();

    // How many family members are in the Smith family?
    QuestionModel howManySmithMembers = DefineQuestionCountingPropertyValues(characters,
      question: "How many members are there in the Smith family?",
      countFilter: model => model.Name.Split(' ').Last() == "Smith",
      distincyBy: model => model.Name);
    
    // How many characters are alive | dead | unknown?
    var characterStatuses = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Status,
      question: (status, character) => $"How many characters are currently **{Enum.GetName(status)}** in Rick and Morty?");

    // How many characters are there of the species {character.Species}?
    List<QuestionModel> characterSpecies = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Species,
      question: (species, character) => $"How many characters are there in the species \"{species}\"?");

    // How many characters originate from {character.Location}?
    List<QuestionModel> characterLocations = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Location.Name,
      question: (location, character) => $"How many characters originate from \"{location}\"?");

    // How many episodes does {character.Name} feature in?
    List<QuestionModel> characterEpisodes = DefineQuestionsByListCount(characters,
      question: character => $"How many episodes does {character.Name} feature in?",
      list: character => character.Episode);

    // How many characters are <gender>?
    List <QuestionModel> characterGenders = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Gender,
      question: (gender, character) => $"How many characters are **{Enum.GetName(gender)}**?");

    return
    [
      howManySmithMembers,
      .. characterStatuses,
      .. characterSpecies,
      .. characterLocations,
      .. characterEpisodes,
      .. characterGenders
    ];
  }
}
