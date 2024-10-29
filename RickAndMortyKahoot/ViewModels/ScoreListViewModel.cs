namespace RickAndMortyKahoot.ViewModels;

public class ScoreListViewModel(KeyValuePair<Guid, int>[] score, bool showHost)
{
  public KeyValuePair<Guid, int>[] Score { get; } = score;
  public bool ShowHost { get; set; }

  public string FormatScore(int score) => score.ToString("N0").Replace(',', '.');
}
