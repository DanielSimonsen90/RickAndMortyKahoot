using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Services.Question;
using RickAndMortyKahoot.Services.Score;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Controllers;
/// <summary>
/// Controller used to handle Home-related requests
/// </summary>
public class HomeController(
  ProjectStore store,
  ScoreService scoreService,
  QuestionService questionService) : Controller
{
  /// <summary>
  /// The home page of the application
  /// </summary>
  /// <param name="userId">Once user is logged in, they're redirected back to index page with userId</param>
  /// <returns>The Index view from /Views/Home</returns>
  public IActionResult Index(Guid? userId)
  {
    // Define the view model
    var vm = new UserViewModel(userId, userId is null || !store.Users.TryGetValue(userId.Value, out User? user) ? null : user);

    // Return Index view
    return View(vm);
  }

  /// <summary>
  /// Form submission for user registration
  /// </summary>
  /// <param name="payload">The payload from the form</param>
  /// <returns>Redirect to <see cref="Index"/></returns>
  [HttpPost("RegisterUser")]
  public IActionResult UserRegisterSubmit(UserPayload payload)
  {
    // Define user from payload
    var user = new User(payload);
    // Add user to store
    store.Users.Add(user.Id, user);

    // Redirect to index page with userId
    return RedirectToAction(nameof(Index), new { userId = user.Id });
  }

  /// <summary>
  /// Clears the cache, so the app doesn't use too much memory in production
  /// </summary>
  /// <returns></returns>
  [HttpGet("ClearCache")]
  public IActionResult ClearCache()
  {
    store.Clear();
    scoreService.Clear();
    questionService.Clear();

    return Ok();
  }
}