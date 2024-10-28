using RickAndMortyKahoot.Models.Users;

namespace RickAndMortyKahoot.ViewModels;

public class UserViewModel(Guid? currentUserId, User? user) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public User? User { get; set; } = user;
}
