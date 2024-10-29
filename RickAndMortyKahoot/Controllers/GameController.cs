using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Services.Score;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Controllers;

[Route("Game")]
public class GameController(
  ProjectStore store,
  ScoreService scoreService) : Controller
{
  [HttpGet("{gameId}")]
  public IActionResult Game(Guid gameId, Guid userId, bool showScore = false)
  {
    if (gameId == Guid.Empty) return BadRequest(nameof(gameId));
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return BadRequest(nameof(game));

    return View(new GameViewModel(userId, game, null, scoreService.GetHighscores(game)));
  }

  [HttpGet("{gameId}/active")]
  public IActionResult ActiveGame(Guid gameId, Guid userId)
  {
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    return View(new GameViewModel(userId, game));
  }

  [HttpGet("QuestionView")]
  public IActionResult QuestionPartialView(Guid gameId, Guid questionId)
  {
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    GameQuestion? question = game.Questions.Find(q => q.Id == questionId);
    if (question is null) return NotFound();

    var vm = new QuestionViewModel(question, game.Questions.Where(q => !q.Available).Count() + 1, game.Questions.Count);

    return PartialView("_Question", vm);
  }

  [HttpPost("CorrectAnswer")]
  public IActionResult CorrectAnswerPartialView([FromBody] CorrectAnswerPayload payload, Guid gameId, Guid userId)
  {
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    if (!store.Users.TryGetValue(userId, out User? user) || user is null) return NotFound();
    if (!game.UserIds.Contains(userId)) return Forbid();

    GameQuestion question = game.CurrentQuestion!;
    Answer answer = new(question.Id, payload.AnswerIndex, userId);

    CorrectAnswerViewModel vm = new(question, answer, payload.Score);

    return PartialView("_CorrectAnswer", vm);
  }
}
