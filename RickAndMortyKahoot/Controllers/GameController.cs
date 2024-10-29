using Microsoft.AspNetCore.Mvc;
using RickAndMortyKahoot.Models.Games;
using RickAndMortyKahoot.Models.Questions;
using RickAndMortyKahoot.Models.Users;
using RickAndMortyKahoot.Services.Score;
using RickAndMortyKahoot.Stores;
using RickAndMortyKahoot.ViewModels;

namespace RickAndMortyKahoot.Controllers;

/// <summary>
/// Controller used to handling Game-related requests
/// </summary>
/// <param name="store">Store to keep active data</param>
/// <param name="scoreService">Service to calculate game scores</param>
[Route("Game")]
public class GameController(
  ProjectStore store,
  ScoreService scoreService) : Controller
{
  /// <summary>
  /// Lobby of game matching <paramref name="gameId"/>
  /// </summary>
  /// <param name="gameId">Id of the game to show lobby of</param>
  /// <param name="userId">Current user id to dynamically render game host functionalities</param>
  /// <returns>The Game view from /Views/Game</returns>
  [HttpGet("{gameId}")]
  public IActionResult Game(Guid gameId, Guid userId)
  {
    // If no gameId, cannot show game lobby
    if (gameId == Guid.Empty) return BadRequest(nameof(gameId));

    // Try parse game from game id. If fails, bad request
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return BadRequest(nameof(game));

    // Return Game view
    return View(new GameViewModel(userId, game, scoreService.GetHighscores(game)));
  }

  /// <summary>
  /// Active game view when game is active
  /// </summary>
  /// <param name="gameId">Id of the game to show the active view of</param>
  /// <param name="userId">Current user id to dynamically render game hos functionalities</param>
  /// <returns>The ActiveGame view from /Views/Game</returns>
  [HttpGet("{gameId}/active")]
  public IActionResult ActiveGame(Guid gameId, Guid userId)
  {
    // Try parse game from id. If fails, not found
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    // If game is inactive, cannot show active view
    if (!game.IsActive) return BadRequest(nameof(game));

    // Return ActiveGame view
    return View(new GameViewModel(userId, game));
  }

  /// <summary>
  /// PartialView of a rendered <see cref="GameQuestion"/>.
  /// This is used in the client.
  /// </summary>
  /// <param name="gameId">Id of the game that contains the question. Used for validation</param>
  /// <param name="questionId">Id of the question to render</param>
  /// <returns>The _Question PartialView from /Views/Game</returns>
  [HttpGet("QuestionView")]
  public IActionResult QuestionPartialView(Guid gameId, Guid questionId)
  {
    // Try parse game from id. If fails, not found
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();

    // Find question from provided questionId. If null, not found
    GameQuestion? question = game.Questions.Find(q => q.Id == questionId);
    if (question is null) return NotFound();

    // Define the necessary ViewModel and return the _Question PartialView
    var vm = new QuestionViewModel(question, game.Questions.Where(q => !q.Available).Count() + 1, game.Questions.Count);
    return PartialView("_Question", vm);
  }

  /// <summary>
  ///  PartialView of a rendered <see cref="GameQuestion"/> with the correct <see cref="Answer"/>.
  ///  This is used in the client.
  /// </summary>
  /// <param name="payload">Payload of the data necessary to display the PartialView</param>
  /// <param name="gameId">Id of the game that contains the question</param>
  /// <param name="userId">Id of the user for validation</param>
  /// <returns>The _CorrectAnswer PartialView from /Views/Game</returns>
  [HttpPost("CorrectAnswer")]
  public IActionResult CorrectAnswerPartialView([FromBody] CorrectAnswerPayload payload, Guid gameId, Guid userId)
  {
    // Try parse game from id. If fails, not found
    if (!store.Games.TryGetValue(gameId, out Game? game) || game is null) return NotFound();
    // Try parse user from id. If fails, not found
    if (!store.Users.TryGetValue(userId, out User? user) || user is null) return NotFound();
    // If user is not in the game, forbid
    if (!game.UserIds.Contains(userId)) return Forbid();

    // Get the current question and the answer from the payload
    GameQuestion question = game.CurrentQuestion!;
    Answer answer = new(question.Id, payload.AnswerIndex, userId);

    // Define the necessary ViewModel and return the _CorrectAnswer PartialView
    CorrectAnswerViewModel vm = new(question, answer, payload.Score);
    return PartialView("_CorrectAnswer", vm);
  }
}
