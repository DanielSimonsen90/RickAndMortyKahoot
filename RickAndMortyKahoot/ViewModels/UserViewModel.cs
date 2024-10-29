using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// ViewModel for the Index view of /Views/Home
/// </summary>
/// <param name="currentUserId">Current user's id</param>
/// <param name="user">User to be displayed</param>
public class UserViewModel(Guid? currentUserId, User? user) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public User? User { get; set; } = user;
}
