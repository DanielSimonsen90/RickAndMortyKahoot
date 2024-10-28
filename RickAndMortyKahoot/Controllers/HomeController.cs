using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Stores;
using System.Text.Json;

namespace RickAndMortyKahoot.Controllers
{
  public class HomeController(ProjectStore store) : Controller
  {
    public IActionResult Index()
    {
#if DEBUG
      if (store.CurrentUser is null) return UserRegisterSubmit(new() { Username = "Danho" });
#endif
      return View(store.CurrentUser);
    }

    [HttpPost]
    public IActionResult UserRegisterSubmit(UserPayload payload)
    {
      var user = new User(payload);
      store.Users.Add(user.Id, user);
      store.CurrentUser = user;
      return RedirectToAction(nameof(Index));
    }

    [HttpGet("Game/{gameId}")]
    public IActionResult Game(Guid gameId)
    {
      if (gameId == Guid.Empty) return BadRequest(nameof(gameId));
      if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return BadRequest(nameof(game));

      return View(game);
    }
  }
}
