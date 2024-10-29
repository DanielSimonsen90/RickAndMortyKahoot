namespace RickAndMortyKahoot.ViewModels;

/// <summary>
/// View model for a _UserList PartialView in /Views/Game
/// </summary>
/// <param name="userIds">List of user ids to display</param>
/// <param name="hostId">Id of the host user</param>
public class UserListViewModel(IEnumerable<Guid> userIds, Guid hostId)
{
  public IEnumerable<Guid> UserIds { get; set; } = userIds;
  public Guid HostId { get; set; } = hostId;
}
