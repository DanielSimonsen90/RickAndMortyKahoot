using RickAndMorty.Net.Api.Models.Domain;
using QuestionModel = RickAndMortyKahoot.Models.Questions.Question;

namespace RickAndMortyKahoot.Services.Question;

public partial class QuestionService
{
  private static List<QuestionModel> DefineEpisodeQuestions(IEnumerable<Episode> episodes)
  {
    // How many episodes
    QuestionModel howManyEpisodesTotal = DefineQuestionsByListCount(episodes,
      question: _ => "How many episodes are there in Rick and Morty?",
      list: _ => episodes,
      take: 1).First();

    // How many characters were in <episode>?
    List<QuestionModel> howManyCharactersInEpisode = DefineQuestionsByListCount(episodes,
      question: episode => $"How many characters were in the episode \"{episode.Name}\" ({episode.EpisodeCode})?",
      list: episode => episode.Characters);

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
