namespace RickAndMortyKahoot.Extensions;

public static class IEnumerableExtensions
{
  public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source, 
    Func<TSource, int, IEnumerable<TSource>, TResult> selector
  ) => source
    .Select(item => selector(item, source.IndexOf(item), source));
  public static IOrderedEnumerable<TSource> OrderBy<TSource, TOrderedBy>(
    this IEnumerable<TSource> source, 
    Func<TSource, int, IEnumerable<TSource>, TOrderedBy> selector
  ) => source
    .OrderBy(item => selector(item, source.IndexOf(item), source));
  public static IOrderedEnumerable<TSource> OrderRandomly<TSource>(
    this IEnumerable<TSource> source
  ) => source.OrderBy((_, __, list) => new Random().Next(list.Count()));
  public static int IndexOf<T>(this IEnumerable<T> source, T item) => source.ToList().IndexOf(item);
}
