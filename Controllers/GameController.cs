using System.Diagnostics;
using launcher.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace launcher.Controllers;

public class GameController : Controller
{
    public string Index(int id)
    {
        return id.ToString();
    }

    public IActionResult Start(int id)
    {
        var game = GameRepository.GetGameById(id);

        if (game == null)
            return NotFound();

        if (game.IsRunning)
            return BadRequest("Game is already running");

        string WorkingDirectory = $"/home/xxxtommystarkxxx/games/{game.FolderName}";
        string script = ".venv/bin/gunicorn";
        string arguments = $"server:app --bind 0.0.0.0:{game.Port} --daemon";

        var startInfo = new ProcessStartInfo
        {
            FileName = script,
            Arguments = arguments,
            WorkingDirectory = WorkingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        try
        {
            var process = Process.Start(startInfo);
            game.PID = process.Id;
            game.IsRunning = true;
        }
        catch (Exception ex)
        {
            return Content("Ошибка запуска: " + ex.Message);
        }

        return Redirect($"http://158.160.142.78:{game.Port}");
    }

    public IActionResult Stop(int id)
    {
        var game = GameRepository.GetGameById(id);

        if (game == null)
            return NotFound();

        if (!game.IsRunning)
            return BadRequest("Game is not running");

        try
        {
            if (game.PID.HasValue)
            {
                Process.GetProcessById(game.PID.Value).Kill();
                game.IsRunning = false;
                game.PID = null;
            }

            return Content("Game stopped successfully");
        }
        catch (Exception ex)
        {
            return Content("Error stopping game: " + ex.Message);
        }
    }
}
