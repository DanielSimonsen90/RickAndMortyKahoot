using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.ViewModels;
using System.Text.Json;

namespace RickAndMortyKahoot.Controllers
{
  public class HomeController(ProjectStore store) : Controller
  {
    public IActionResult Index(Guid? userId)
    {
#if false
      if (store.CurrentUser is null) return UserRegisterSubmit(new() { Username = "Danho" });
#endif
      var vm = new UserViewModel(userId, store.TemporaryUser);
      store.TemporaryUser = null;
      return View(vm);
    }

    [HttpPost("RegisterUser")]
    public IActionResult UserRegisterSubmit(UserPayload payload)
    {
      var user = new User(payload);
      store.Users.Add(user.Id, user);
      store.TemporaryUser = user;
      return RedirectToAction(nameof(Index), new { userId = user.Id });
    }
  }
}
