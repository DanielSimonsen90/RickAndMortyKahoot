using RickAndMorty.Net.Api.Models.Domain;
using RickAndMortyKahoot.Utils;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;

namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  public static List<QuestionModel> DefineEpisodeQuestions(IEnumerable<Episode> episodes)
  {
    // How many episodes
    QuestionModel howManyEpisodesTotal = DefineQuestions(episodes,
      question: _ => "How many episodes are there in Rick and Morty?",
      questionAroundProp: _ => episodes.Count(),
      amountOfQuestions: 1).First();

    // How many characters were in <episode>?
    List<QuestionModel> howManyCharactersInEpisode = DefineQuestions(episodes,
      question: episode => $"How many characters were in the episode \"{episode.Name}\" ({episode.EpisodeCode})?",
      questionAroundProp: episode => episode.Characters.Count());

    // When did <episode> air?
    List<QuestionModel> whenDidEpisodeAir = DefineQuestions(episodes,
      question: episode => $"When did the episode \"{episode.Name}\" ({episode.EpisodeCode}) air?",
      questionAroundProp: episode => episode.AirDate);

    // What season and episode is <episode.name>?
    List<QuestionModel> whatSeasonAndEpisodeIsEpisode = DefineQuestions(episodes,
      question: episode => $"What season and episode is \"{episode.Name}\"?",
      questionAroundProp: episode => episode.EpisodeCode);

    return
    [
       howManyEpisodesTotal,
      .. howManyCharactersInEpisode,
      .. whenDidEpisodeAir,
      .. whatSeasonAndEpisodeIsEpisode
    ];
  }
}
