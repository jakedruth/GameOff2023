using Godot;
using System;

public partial class LevelSelect : Control
{
    [Export] Container container;
    [Export] Button firstButton;

    public override void _Ready()
    {
        firstButton.GrabFocus();

    }

    public void TestSave()
    {
        GameData data = new GameData();
        GD.Print(data);
        data.SetValue("level01Completed", true);
        SaveLoadSystem.Save(data);
    }

    public void TestLoad()
    {
        GameData data = SaveLoadSystem.Load();
        GD.Print(data.data["level01Completed"]);
    }
}
