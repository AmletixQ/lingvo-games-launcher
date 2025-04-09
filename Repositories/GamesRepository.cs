using System.Collections.Generic;
using launcher.Models;

namespace launcher.Repositories;

class GameRepository
{
    static int gamesCount = 0;
    static readonly List<GameModel> gameModels =
    [
        new GameModel(gamesCount++, "Piece by Piece", 5001, false, "piece-by-piece"),
    ];

    public static List<GameModel> GetAllGames()
    {
        return gameModels;
    }

    public static GameModel? GetGameById(int id)
    {
        return gameModels.Find(game => game.Id == id);
    }
}
