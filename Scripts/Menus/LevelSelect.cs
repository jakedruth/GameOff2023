using Godot;
using System;

public partial class LevelSelect : Control
{
    [Export] Container container;
    [Export] Button firstButton;

    public override void _Ready()
    {
        Node mainLevelInfoHolder = container.GetChild(0);

        SceneManager manager = GetNode<SceneManager>("/root/SceneManager");
        int count = manager.GetNumberOfLevels();

        firstButton.GrabFocus();

        for (int i = 0; i < count; i++)
        {
            if (i != 0)
            {
                Node duplicate = mainLevelInfoHolder.Duplicate(0);
                container.AddChild(duplicate);
            }

            bool unlocked = (i == 0)
                ? true
                : manager.GameData.GetValue<bool>($"level{i}");
            bool beaten = manager.GameData.GetValue<bool>($"level{i + 1}");

            LevelInfo info = manager.GetLevelInfo(i);
            Node holder = container.GetChild(i);

            Label title = holder.GetChild<Label>(1);
            Label data = holder.GetChild<Label>(2);
            Button button = holder.GetChild<Button>(3);

            title.Text = info.name;
            data.Text = $"[{(unlocked ? "x" : "")}] unlocked\n[{(beaten ? "x" : "")}] beaten";
            button.Disabled = !unlocked;
            button.Text = unlocked ? "Play" : "Locked";
            button.ZIndex = i;
            button.Pressed += () =>
            {
                int index = button.ZIndex;
                manager.GoToLevel(index);
            };
        }
    }

    public void GoToMainMenu()
    {
        GetNode<SceneManager>("/root/SceneManager").GoToMainMenu();
    }
}
