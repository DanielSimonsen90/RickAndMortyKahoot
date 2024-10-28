﻿using Microsoft.AspNetCore.SignalR;
using RickAndMortyKahoot.Models.Exceptions;
using RickAndMortyKahoot.Services.Question;
using RickAndMortyKahoot.Stores;
namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub(
  ProjectStore store, 
  QuestionService questionService) : Hub
{
  private async Task OnRecieveAction(string actionName, Func<Task> action)
  {
    try
    {
      await action();
    }
    catch (Exception ex)
    {
      await DispatchError(actionName, ex);
    }
  }

  private async Task DispatchHubEvent(Guid gameId, string eventName, params object[] args)
  {
    await Clients.Group(gameId.ToString()).SendCoreAsync(eventName, args);
    //await Clients.All.SendAsync(eventName, args);
  }
  private async Task DispatchError(string actionName, Exception ex)
  {
    await Clients.Client(Context.ConnectionId).SendCoreAsync(Events.ERROR, [actionName, ex]);
  }
}
