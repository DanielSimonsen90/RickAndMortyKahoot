namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// View model for the _ScoreList PartialView in /Views/Game
/// </summary>
/// <param name="score">Current scores of the game</param>
public class ScoreListViewModel(KeyValuePair<Guid, int>[] score, Guid? hostId)
{
  public KeyValuePair<Guid, int>[] Score { get; } = score;

  /// <summary>
  /// If defined, the host user will be displayed differently
  /// </summary>
  public Guid? HostId { get; set; } = hostId;

  /// <summary>
  /// Formats the score into a string with a thousand separator
  /// </summary>
  /// <param name="score">Score value</param>
  public string FormatScore(int score) => score.ToString("N0").Replace(',', '.');
}
