using Godot;

[GlobalClass]
public partial class LevelInfo : Resource
{
    [Export] public string path { get; private set; }
    [Export] public string name { get; private set; }

    public LevelInfo()
    {
        path = "";
        name = "";
    }
}