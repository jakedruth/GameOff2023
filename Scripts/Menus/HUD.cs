using Godot;
using System;

public partial class HUD : Control
{
    public enum ButtonSelector { SHRINK, NORMAL, GROW }

    [ExportGroup("Select Character")]
    [Export] private Control selectCharacterHolder;
    [Export] private Button shrinkButton;
    [Export] private Button normalButton;
    [Export] private Button GrowButton;
    [ExportGroup("Level Complete")]
    [Export] private Control levelCompleteHolder;
    [Export] private Button levelCompleteButton;
    [ExportGroup("Pause Screen")]
    [Export] private Control pauseScreenHolder;
    [Export] private Button continueGameButton;

    public override void _Ready()
    {
        HideCharacterSelect();
        HideLevelComplete();
        HidePauseMenu();
        Show();
    }

    public void DisplayCharacterSelect()
    {
        selectCharacterHolder.Show();
        selectCharacterHolder.ProcessMode = ProcessModeEnum.Inherit;
        if (!shrinkButton.Disabled)
            shrinkButton.GrabFocus();
        else if (!normalButton.Disabled)
            normalButton.GrabFocus();
        else
            GrowButton.GrabFocus();
    }

    public void HideCharacterSelect()
    {
        selectCharacterHolder.Hide();
        selectCharacterHolder.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void ShowPauseMenu()
    {
        pauseScreenHolder.Show();
        pauseScreenHolder.ProcessMode = ProcessModeEnum.Inherit;
        continueGameButton.GrabFocus();
    }

    public void HidePauseMenu()
    {
        pauseScreenHolder.Hide();
        pauseScreenHolder.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void SetSizeButtonDisabled(ButtonSelector selector, bool value)
    {
        Button refButton;
        switch (selector)
        {
            default:
            case ButtonSelector.SHRINK:
                refButton = shrinkButton;
                break;
            case ButtonSelector.NORMAL:
                refButton = normalButton;
                break;
            case ButtonSelector.GROW:
                refButton = GrowButton;
                break;
        }

        refButton.Disabled = value;

        if (value)
            refButton.GetChild<Node2D>(0).Hide();
        else
            refButton.GetChild<Node2D>(0).Show();
    }

    public void DisplayLevelComplete()
    {
        levelCompleteHolder.Show();
        levelCompleteHolder.ProcessMode = ProcessModeEnum.Inherit;
        levelCompleteButton.GrabFocus();
    }

    public void HideLevelComplete()
    {
        levelCompleteHolder.Hide();
        levelCompleteHolder.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void OnPlayLevel(int size)
    {
        HideCharacterSelect();
        GetParent<LevelHandler>().SelectSize(size);
    }

    public void UnPause()
    {
        GetNode<SceneManager>("/root/SceneManager").UnPause();
    }

    public void GoToLevelSelect()
    {
        GetNode<SceneManager>("/root/SceneManager").GoToLevelSelect();
    }

    public void ResetLevel()
    {
        GetNode<SceneManager>("/root/SceneManager").ResetLevel();
    }
}
