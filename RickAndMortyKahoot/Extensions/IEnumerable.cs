namespace RickAndMortyKahoot.Extensions;

/// <summary>
/// Custom extensions for <see cref="IEnumerable{T}"/>
/// </summary>
public static class IEnumerableExtensions
{
  /// <summary>
  /// IEnumerable.Select with index and source
  /// </summary>
  /// <typeparam name="TSource">Source type of the IEnumerable</typeparam>
  /// <typeparam name="TResult">End result of the IEnumerable select</typeparam>
  /// <param name="source">"this" IEnumerable</param>
  /// <param name="selector">Selector callback</param>
  /// <returns>The selected value from <paramref name="selector"/></returns>
  public static IEnumerable<TResult> Select<TSource, TResult>(
    this IEnumerable<TSource> source, 
    Func<TSource, int, IEnumerable<TSource>, TResult> selector
  ) => source
    .Select(item => selector(item, source.IndexOf(item), source));

  /// <summary>
  /// IEnumerable.OrderBy with index and source
  /// </summary>
  /// <typeparam name="TSource">Source type of the IEnumerable</typeparam>
  /// <typeparam name="TOrderedBy">Type to order by</typeparam>
  /// <param name="source">"this" IEnumerable</param>
  /// <param name="selector">Selector callback</param>
  public static IOrderedEnumerable<TSource> OrderBy<TSource, TOrderedBy>(
    this IEnumerable<TSource> source, 
    Func<TSource, int, IEnumerable<TSource>, TOrderedBy> selector
  ) => source
    .OrderBy(item => selector(item, source.IndexOf(item), source));

  /// <summary>
  /// Order the IEnumerable randomly
  /// </summary>
  /// <remarks>
  /// This uses <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, int, IEnumerable{TSource}, TKey})"/>
  /// </remarks>
  /// <typeparam name="TSource">Source type of the IEnumerable</typeparam>
  /// <param name="source">"this" IEnumerable</param>
  /// <returns>The IEnumerable ordered randomly</returns>
  public static IOrderedEnumerable<TSource> OrderRandomly<TSource>(
    this IEnumerable<TSource> source
  ) => source.OrderBy((_, __, list) => new Random().Next(list.Count()));

  /// <summary>
  /// Get the index of an item in an IEnumerable
  /// </summary>
  /// <typeparam name="T">Typeof IEnumerable</typeparam>
  /// <param name="source">"this" IEnumerable</param>
  /// <param name="item">Item to find index of</param>
  /// <returns>The index of <paramref name="item"/> in <paramref name="source"/></returns>
  public static int IndexOf<T>(this IEnumerable<T> source, T item) => source.ToList().IndexOf(item);
}
