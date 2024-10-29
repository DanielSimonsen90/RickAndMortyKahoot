using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Controllers;

[Route("Game")]
public class GameController(ProjectStore store) : Controller
{
  [HttpGet("{gameId}")]
  public IActionResult Game(Guid gameId, Guid userId)
  {
    if (gameId == Guid.Empty) return BadRequest(nameof(gameId));
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return BadRequest(nameof(game));

    return View(new GameViewModel(userId, game));
  }

  [HttpGet("{gameId}/active")]
  public IActionResult ActiveGame(Guid gameId, Guid userId)
  {
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    return View(new GameViewModel(userId, game));
  }

  [HttpGet("QuestionView")]
  public IActionResult QuestionView(Guid gameId, Guid questionId)
  {
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    GameQuestion? question = game.Questions.Find(q => q.Id == questionId);
    if (question is null) return NotFound();

    return PartialView("_QuestionView", question);
  }
}
