using Godot;

[System.Serializable]
public class GameData
{
    public Godot.Collections.Dictionary<string, Variant> data = new Godot.Collections.Dictionary<string, Variant>
    {
        {"level01Completed", false},
        {"level02Completed", false},
        {"level03Completed", false},
        {"level04Completed", false},
        {"level05Completed", false},
        {"level06Completed", false},
        {"level07Completed", false},
        {"level08Completed", false},
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