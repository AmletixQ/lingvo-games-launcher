namespace launcher.Models;

class GameModel(int id, string name, int port, bool isRunning, string folderName)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public int Port { get; set; } = port;
    public bool IsRunning { get; set; } = isRunning;
    public string FolderName { get; set; } = folderName;
}
