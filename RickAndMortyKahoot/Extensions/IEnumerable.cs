namespace RickAndMortyKahoot.Extensions;

public static class IEnumerableExtensions
{
  public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TSource>, TResult> selector) => source
    .Select(item => selector(item, source.IndexOf(item), source));

  public static int IndexOf<T>(this IEnumerable<T> source, T item) => source.ToList().IndexOf(item);
}
