namespace RickAndMortyKahoot.ViewModels;

public class UserListViewModel(IEnumerable<Guid> userIds, Guid hostId)
{
  public IEnumerable<Guid> UserIds { get; set; } = userIds;
    public Guid HostId { get; set; } = hostId;
}
