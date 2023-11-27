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

        file.Flush();
        file.Close();
    }

    public static bool DoesSaveExist()
    {
        return FileAccess.FileExists(localPath);
    }

    public static void Load(ref GameData data)
    {
        if (!DoesSaveExist())
        {
            GD.PrintErr($"there is no file at {localPath}. Making new file");
            data = new GameData();
            Save(data);
            return;
        }

        FileAccess file = FileAccess.Open(localPath, FileAccess.ModeFlags.Read);
        string line = file.GetLine();
        var json = new Json();
        var parseResults = json.Parse(line);
        if (parseResults != Error.Ok)
        {
            GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {line} at line {json.GetErrorLine()}");

            return;
        }

        data.data = (Godot.Collections.Dictionary<string, Variant>)json.Data;

        file.Flush();
        file.Close();
    }

    public static void DeleteSaveDataFile()
    {
        DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(localPath));
    }
}