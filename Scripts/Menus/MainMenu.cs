using Godot;
using System;

public partial class MainMenu : Control
{
    public void OnPlayPressed()
    {
        GetNode<SceneManager>("/root/SceneManager").GoToLevel(0);
    }

    public void OnQuitPressed()
    {
        GetNode<SceneManager>("/root/SceneManager").QuitGame();
    }
}
