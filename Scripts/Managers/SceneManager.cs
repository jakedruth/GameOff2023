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
        if (CurrentSceneIndex < 0)
            return;

        GoToLevel(CurrentSceneIndex);
    }

    public void GoToMainMenu()
    {
        PackedScene nextScene = (_buildSettings as BuildSettings).GetMainMenu();
        CallDeferred(MethodName.DefferedGoToLevel, nextScene);
    }

    public void GoToLevelSelect()
    {
        PackedScene nextScene = (_buildSettings as BuildSettings).GetLevelSelect();
        CallDeferred(MethodName.DefferedGoToLevel, nextScene);
    }

    public void GoToLevel(int index)
    {
        // Get the next scene from the build settings
        PackedScene nextScene = (_buildSettings as BuildSettings).GetLevel(index);

        CallDeferred(MethodName.DefferedGoToLevel, nextScene, index);
    }

    private void DefferedGoToLevel(PackedScene nextScene, int index = -1)
    {
        CurrentSceneIndex = index;

        // Free the CurrentScene from memeory
        CurrentScene.Free();

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
