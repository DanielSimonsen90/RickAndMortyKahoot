using Microsoft.AspNetCore.SignalR;
using RickAndMortyKahoot.Services.Question;
using RickAndMortyKahoot.Services.Score;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.Models.Questions;
namespace RickAndMortyKahoot.Hubs.Kahoot;

/// <summary>
/// Kahoot hub for managing "Kahoot" games
/// </summary>
/// <param name="store">Store to save data to cache</param>
/// <param name="questionService">Service used to recieve <see cref="GameQuestion"/>s</param>
/// <param name="scoreService">Service to calculate the scores for the game</param>
public partial class KahootHub(
  ProjectStore store, 
  QuestionService questionService,
  ScoreService scoreService) : Hub
{
  /// <summary>
  /// Action wrapper for recieving actions
  /// </summary>
  /// <remarks>
  /// This method should be used to wrap all actions to ensure that errors are caught and dispatched to the client
  /// </remarks>
  /// <param name="actionName">Name of the action that is wrapped</param>
  /// <param name="action">Action to be executed</param>
  private async Task OnRecieveAction(string actionName, Func<Task> action)
  {
    try
    {
      await action();
    }
    catch (Exception ex) // Catch all exceptions and dispatch error
    {
      await DispatchError(actionName, ex);
    }
  }

  /// <summary>
  /// Dispatch an event to all clients
  /// </summary>
  /// <param name="gameId">Game id so clients can manage the event depending on relevance</param>
  /// <param name="eventName">Name of the event to dispatch</param>
  /// <param name="args">Arguments to send with the event</param>
  private async Task DispatchHubEvent(Guid gameId, string eventName, params object[] args)
  {
    // Usage of SendCoreAsync to allow endless arguments
    await Clients.All.SendCoreAsync(eventName, args.Prepend(gameId).ToArray());
  }

  /// <summary>
  /// Dispatch an error to the client
  /// </summary>
  /// <param name="actionName">Name of the action that caused the error</param>
  /// <param name="ex">Exception that was thrown</param>
  private async Task DispatchError(string actionName, Exception ex)
  {
    // Dispatch error event to the client that called the action
    // Exceptions cannot be serialized to JSON, so custom object is created to map the exception
    await Clients.Client(Context.ConnectionId).SendAsync(Events.ERROR, actionName, new 
    { 
      message = ex.Message, 
      stack = ex.StackTrace,
      inner = ex.InnerException,
      name = ex.GetType().Name,
    });
  }
}
