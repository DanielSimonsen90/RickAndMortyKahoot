using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Models.Games;
#nullable disable

/// <summary>
/// Payload for joining a <see cref="Game"/>
/// </summary>
/// <param name="currentUserId">Id of the user that wishes to join the game</param>
public class JoinGamePayload(Guid currentUserId) : IViewModel
{
  public Guid? CurrentUserId => currentUserId;
  public Guid? InviteCode { get; set; } = null;
}
