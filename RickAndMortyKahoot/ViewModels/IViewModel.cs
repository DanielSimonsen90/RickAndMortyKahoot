namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// Base interface to recieve the current user id
/// </summary>
public interface IViewModel
{
  Guid? CurrentUserId { get; }
}
