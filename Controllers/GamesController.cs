using launcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace launcher.Controllers;

public class GamesController : Controller
{
    public IActionResult Index()
    {
        var games = GameRepository.GetAllGames();
        return View(games);
    }
}
