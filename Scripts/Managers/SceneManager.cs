using Godot;
using System;

[Tool]
public partial class SceneManager : Node
{
    private GameData _gameData;
    public GameData GameData { get { return _gameData; } }
    private Resource _buildSettings;

    public Node CurrentScene { get; private set; }
    public int CurrentLevelIndex { get; private set; } = -2;

    public override void _Ready()
    {
        // Get the build settings
        _buildSettings = GD.Load("res://Settings/BuildSettings.tres");

        // Autoloaded nodes are always first, the last child of root is always the loaded scene.
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);

        _gameData = new GameData();
        //SaveLoadSystem.Save(_gameData);
        SaveLoadSystem.Load(ref _gameData);
    }

    public void ResetLevel()
    {
        if (CurrentLevelIndex < 0)
            return;

        GoToLevel(CurrentLevelIndex);
    }

    public void GoToMainMenu()
    {
        PackedScene nextScene = (_buildSettings as BuildSettings).GetMainMenu();
        CallDeferred(MethodName.DefferedGoToLevel, nextScene, -2);
    }

    public void GoToLevelSelect()
    {
        PackedScene nextScene = (_buildSettings as BuildSettings).GetLevelSelect();
        CallDeferred(MethodName.DefferedGoToLevel, nextScene, -1);
    }

    public void GoToLevel(int index)
    {
        // Get the next scene from the build settings
        PackedScene nextScene = (_buildSettings as BuildSettings).GetLevel(index);

        CallDeferred(MethodName.DefferedGoToLevel, nextScene, index);
    }

    private void DefferedGoToLevel(PackedScene nextScene, int index)
    {
        CurrentLevelIndex = index;

        // Free the CurrentScene from memeory
        CurrentScene.Free();

        // Instantiate the next scene
        CurrentScene = nextScene.Instantiate();

        // Add it to the active scene, as child of root.
        GetTree().Root.AddChild(CurrentScene);

        // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
        GetTree().CurrentScene = CurrentScene;

        GetTree().Paused = false;
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }

    public BuildSettings GetBuildSettings()
    {
        return (BuildSettings)_buildSettings;
    }

    public void BeatCurrentLevel()
    {
        string level = $"level{CurrentLevelIndex + 1}";
        _gameData.SetValue(level, true);

        SaveGame();
    }

    public void DeleteSaveFile()
    {
        SaveLoadSystem.DeleteSaveDataFile();
        _gameData = new GameData();
        SaveLoadSystem.Save(_gameData);
    }

    public void SaveGame()
    {
        SaveLoadSystem.Save(_gameData);
    }

    public void TogglePause()
    {
        LevelHandler levelHandler = (LevelHandler)GetTree().GetFirstNodeInGroup("LevelHandler");
        levelHandler?.TogglePause();
    }

    public void Pause()
    {
        LevelHandler levelHandler = (LevelHandler)GetTree().GetFirstNodeInGroup("LevelHandler");
        levelHandler?.Pause();
    }

    public void UnPause()
    {
        LevelHandler levelHandler = (LevelHandler)GetTree().GetFirstNodeInGroup("LevelHandler");
        levelHandler?.UnPause();
    }
}
