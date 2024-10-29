using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Models.Games;
#nullable disable

public class JoinGamePayload(Guid currentUserId) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public Guid? InviteCode { get; set; } = null;
}
