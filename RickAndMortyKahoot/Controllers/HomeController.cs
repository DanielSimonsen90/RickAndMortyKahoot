using Microsoft.AspNetCore.Mvc;

namespace RickAndMortyKahoot.Controllers
{
    public class HomeController() : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
