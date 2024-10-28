using RickAndMorty.Net.Api.Models.Domain;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;

namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  public static List<QuestionModel> DefineCharacterQuestions(IEnumerable<Character> characters)
  {
    // How many characters
    QuestionModel howManyCharacters = DefineQuestions(characters,
      question: _ => "How many characters rae there in Rick and Morty?",
      questionAroundProp: _ => characters.Count(),
      amountOfQuestions: 1).First();

    // How many family members are in the Smith family?
    QuestionModel howManySmithMembers = DefineQuestions(characters,
      question: _ => $"How many members are there in the Smith family?",
      questionAroundProp: character => character.Name,
      filter: name => name.Split(' ').Last() == "Smith",
      amountOfQuestions: 1).First();

    // How many characters are alive | dead | unknown?
    List<QuestionModel> characterStatuses = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Status,
      question: (status, character) => $"How many characters are currently **{Enum.GetName(status)}** in Rick and Morty?");

    // How many characters are there of the species {character.Species}?
    List<QuestionModel> characterSpecies = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Species,
      question: (species, character) => $"How many characters are there in the species \"{species}\"?");

    // How many characters originate from {character.Location}?
    List<QuestionModel> characterLocations = DefineQuestionsFromPropertyValues(characters,
      questionAroundProp: character => character.Location,
      question: (location, character) => $"How many characters originate from \"{location.Name}\"?");

    // How many episodes does {character.Name} feature in?
    List<QuestionModel> characterEpisodes = DefineQuestions(characters,
      question: character => $"How many episodes does {character.Name} feature in?",
      questionAroundProp: character => character.Episode.Count());

    // How many characters are <gender>?
    List<QuestionModel> characterGenders = DefineQuestionsFromPropertyValues(characters,
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
