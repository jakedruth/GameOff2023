using Godot;
using System;

public partial class MainMenu : Control
{
    public void OnPlayPressed()
    {
        GetNode<SceneManager>("/root/SceneManager").GoToLevelSelect();
    }

    public void OnClearDataPressed()
    {
        GetNode<SceneManager>("/root/SceneManager").DeleteSaveFile();
    }

    public void OnQuitPressed()
    {
        GetNode<SceneManager>("/root/SceneManager").QuitGame();
    }
}
