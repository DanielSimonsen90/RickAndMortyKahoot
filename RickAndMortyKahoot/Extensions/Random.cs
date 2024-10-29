using RickAndMortyKahoot.Extensions;

namespace RickAndMortyKahoot.Extensions;

/// <summary>
/// Extensions for <see cref="Random"/>
/// </summary>
public static class RandomExtensions
{
  /// <summary>
  /// Get random choices around <paramref name="answer"/>
  /// </summary>
  /// <param name="random">The <see cref="Random"/> instance</param>
  /// <param name="answer">Answer to get choices around</param>
  /// <returns>Random choices around <paramref name="answer"/></returns>
  public static IEnumerable<string> GetChoicesAround(this Random random, int answer) => Enumerable
    .Range(0, 3) // Select 3 random choices
    .Select((_, __, list) =>
    {
      int value = -1; // Default value to -1
      // While value is in list or equal to answer, get a new random value
      do value = random.Next(answer + random.Next(25)); while (list.Contains(value) || value == answer);
      // Return random value
      return value;
    })
    .Select(c => c.ToString());
}
