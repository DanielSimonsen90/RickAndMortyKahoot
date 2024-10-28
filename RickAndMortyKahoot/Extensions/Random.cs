using RickAndMorty.Net.Api.Models.Domain;

namespace RickAndMortyKahoot.Utils;

public static class RandomExtensions
{
  public static IEnumerable<string> GetChoicesAround(this Random random, int answer) => Enumerable
  .Range(1, 3)
  .Select(_ => random.Next(answer + random.Next(25)).ToString());

  public static IEnumerable<T> SelectRandomUniqueItems<T>(this Random random, IEnumerable<T> list, int max) => list
    .OrderBy(_ => random.Next())
    .Distinct()
    .Take(max);
  public static IEnumerable<TMapped> SelectRandomUniqueItems<T, TMapped>(this Random random, IEnumerable<T> list, int max,  Func<T, TMapped> mapper) => random
    .SelectRandomUniqueItems(list, max)
    .Select(mapper);
}
