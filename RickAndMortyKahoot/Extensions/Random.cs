using RickAndMorty.Net.Api.Models.Domain;
using RickAndMortyKahoot.Extensions;

namespace RickAndMortyKahoot.Utils;

public static class RandomExtensions
{
  public static IEnumerable<string> GetChoicesAround(this Random random, int answer) => Enumerable
  .Range(0, 4)
  .Select((_, __, list) =>
  {
    int value = -1;
    do value = random.Next(answer + random.Next(25)); while (list.Contains(value) || value == answer);
    return value;
  })
  .Select(c => c.ToString());

  public static IEnumerable<T> SelectRandomUniqueItems<T>(this Random random, IEnumerable<T> list, int max) => list
    .OrderBy(_ => random.Next())
    .Distinct()
    .Take(max);
  public static IEnumerable<TMapped> SelectRandomUniqueItems<T, TMapped>(this Random random, IEnumerable<T> list, int max,  Func<T, TMapped> mapper) => random
    .SelectRandomUniqueItems(list, max)
    .Select(mapper);
}
