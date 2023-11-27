using Godot;

[System.Serializable]
public class GameData
{
    public Godot.Collections.Dictionary<string, Variant> data = new Godot.Collections.Dictionary<string, Variant>
    {
        {"level1", false},
        {"level2", false},
        {"level3", false},
        {"level4", false},
        {"level5", false},
        {"level6", false},
        {"level7", false},
        {"level8", false},
    };

    public Variant GetValue(string key)
    {
        return data[key];
    }

    public T GetValue<[MustBeVariant] T>(string key)
    {
        return GetValue(key).As<T>();
    }

    public void SetValue(string key, Variant value)
    {
        if (data.ContainsKey(key))
            data[key] = value;
        else
            GD.PrintErr($"Key '{key}' was not found");
    }
}