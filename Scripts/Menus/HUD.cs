using Godot;
using System;

public partial class HUD : Control
{
    private Control _selectCharacterHolder;

    public override void _Ready()
    {
        _selectCharacterHolder = GetChild(0) as Control;
    }

    public void DisplayCharacterSelect()
    {
        _selectCharacterHolder.Show();
        _selectCharacterHolder.ProcessMode = ProcessModeEnum.Inherit;
    }

    public void HideCharacterSelect()
    {
        GD.Print("Here");
        _selectCharacterHolder.Hide();
        _selectCharacterHolder.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void OnPlayLevel(int size)
    {
        GD.Print($"HUD.OnPlayLevel() : {size}");

        HideCharacterSelect();

        GetParent<LevelHandler>().SelectSize(size);
    }
}
