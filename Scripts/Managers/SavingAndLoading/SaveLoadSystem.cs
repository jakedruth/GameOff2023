using Godot;

public static class SaveLoadSystem
{
    private static readonly string fileName = "data.sav";
    private static readonly string localPath = $"user://{fileName}";

    public static void Save(GameData data)
    {
        using FileAccess file = FileAccess.Open(localPath, FileAccess.ModeFlags.Write);
        string jsonString = Json.Stringify(data.data);
        file.StoreLine(jsonString);

        GD.Print("saved game");
    }

    public static GameData Load()
    {
        GameData result = new GameData();

        if (!FileAccess.FileExists(localPath))
        {
            throw new System.Exception($"there is no file at {localPath}");
        }

        FileAccess file = FileAccess.Open(localPath, FileAccess.ModeFlags.Read);
        string line = file.GetLine();
        var json = new Json();
        var parseResults = json.Parse(line);
        if (parseResults != Error.Ok)
        {
            GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {line} at line {json.GetErrorLine()}");
            return null;
        }

        result.data = (Godot.Collections.Dictionary<string, Variant>)json.Data;

        return result;
    }
}