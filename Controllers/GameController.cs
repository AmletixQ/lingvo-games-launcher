using System.Diagnostics;
using launcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace launcher.Controllers;

public class GameController : Controller
{
    private readonly string BASE_PATH = "/home/xxxtommystarkxxx/games/";
    private readonly string BASE_IP = "http://158.160.142.78";

    public IActionResult Index(int id)
    {
        var game = GameRepository.GetGameById(id);

        return View(game);
    }

    public IActionResult Start(int id)
    {
        var game = GameRepository.GetGameById(id);

        if (game == null)
            return NotFound();

        if (game.IsRunning)
            return BadRequest("Game is already running");

        string runningScript = Path.Combine(BASE_PATH, game.FolderName, "run.sh");

        if (!System.IO.File.Exists(runningScript))
            return NotFound("Game script not found");

        var runningProcess = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = runningScript,
            WorkingDirectory = Path.GetDirectoryName(runningScript),
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            Process.Start(runningProcess);
            game.IsRunning = true;

            return Redirect($"{BASE_IP}:{game.Port}");
        }
        catch (Exception ex)
        {
            return BadRequest("Error starting game: " + ex.Message);
        }
    }

    public IActionResult Stop(int id)
    {
        var game = GameRepository.GetGameById(id);

        if (game == null)
            return NotFound();

        if (game.IsRunning)
            return BadRequest("Game is already running");

        string stopingPath = Path.Combine(BASE_PATH, game.FolderName, "stop.sh");

        if (!System.IO.File.Exists(stopingPath))
            return NotFound("Game script not found");

        var stoppingProcess = new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = stopingPath,
            WorkingDirectory = Path.GetDirectoryName(stopingPath),
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            Process.Start(stoppingProcess);
            game.IsRunning = false;

            return Redirect($"{BASE_IP}:5000");
        }
        catch (Exception ex)
        {
            return BadRequest("Error stopping game: " + ex.Message);
        }
    }
}
