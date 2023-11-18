using Godot;
using System;

[Tool]
public partial class SceneManager : Node
{
    [Export] public string[] levels;
    public Node CurrentScene { get; private set; }

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
    }

    public void GoToScene(string path)
    {
        CallDeferred(MethodName.DefferedGoToScene, path);
    }

    public void DefferedGoToScene(string path)
    {
        CurrentScene.Free();

        var nextScene = GD.Load<PackedScene>(path);

        CurrentScene = nextScene.Instantiate();

        GetTree().Root.AddChild(CurrentScene);

        GetTree().CurrentScene = CurrentScene;
    }
}
