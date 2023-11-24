using Godot;
using System;

[Tool]
public partial class SceneManager : Node
{
    private Resource _buildSettings;

    public Node CurrentScene { get; private set; }
    public int CurrentSceneIndex { get; private set; }

    public override void _Ready()
    {
        // Get the build settings
        _buildSettings = GD.Load("res://Settings/BuildSettings.tres");

        // Autoloaded nodes are always first, the last child of root is always the loaded scene.
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);

    }

    public void ResetLevel()
    {
        GoToScene(CurrentSceneIndex);
    }

    public void GoToScene(int index)
    {
        CallDeferred(MethodName.DefferedGoToScene, index);
    }

    private void DefferedGoToScene(int index)
    {
        CurrentSceneIndex = index;

        // Free the CurrentScene from memeory
        CurrentScene.Free();

        // Get the next scene from the build settings
        PackedScene nextScene = (_buildSettings as BuildSettings).GetLevel(index);

        // Instantiate the next scene
        CurrentScene = nextScene.Instantiate();

        // Add it to the active scene, as child of root.
        GetTree().Root.AddChild(CurrentScene);

        // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
        GetTree().CurrentScene = CurrentScene;
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
