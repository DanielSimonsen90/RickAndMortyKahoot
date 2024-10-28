using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Stores;
using System.Text.Json;

namespace RickAndMortyKahoot.Controllers
{
  public class HomeController(ProjectStore store) : Controller
  {
    public IActionResult Index()
    {
      if (TempData["User"] is not string json) return View();
      
      return View(JsonSerializer.Deserialize<User>(json));
    }

    [HttpPost]
    public IActionResult UserRegisterSubmit(UserPayload payload)
    {
      var user = new User(payload);
      store.Users.Add(user);
      TempData["User"] = JsonSerializer.Serialize(user);
      return RedirectToAction(nameof(Index));
    }
  }
}
