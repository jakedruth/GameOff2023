using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelHandler : Node2D
{
    private HUD _hud;

    public int Round { get; private set; }
    public int CurrentSize { get; private set; }

    private PlayerController _player;
    private Vector2 _playerSpawnPoint;

    [Export] Node stageHolder { get; set; }
    private PackedScene _roundScene;

    [Signal] public delegate void OnRoundStartEventHandler(int round);

    public override void _Ready()
    {
        _hud = GetChild<HUD>(0);
        _hud.DisplayCharacterSelect();

        _player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
        if (_player == null)
            throw new Exception("Could not locate player ndoe");

        _playerSpawnPoint = _player.GlobalPosition;
        Round = 1;
        CurrentSize = -1;

        SetUpRound();
    }

    public void CompletedRound()
    {
        Round++;
        if (Round > 3)
        {
            CompletedLevel();
            return;
        }

        SetUpRound();
    }

    private void CompletedLevel()
    {
        GetNode<SceneManager>("/root/SceneManager").BeatCurrentLevel();

        _player.StateMachine.TransitionToState("idle");
        _hud.DisplayLevelComplete();
    }

    private void SetUpRound()
    {
        UnPause();
        _player.StateMachine.TransitionToState("idle");
        _hud.DisplayCharacterSelect();

        _player.GlobalPosition = _playerSpawnPoint;
        // foreach (Node node in allRoundNodes)
        // {
        //     if (!node.IsInsideTree())
        //         continue;

        //     Node[] roundNodes = Round == 0
        //             ? round1nodes
        //             : Round == 1
        //                 ? round2nodes
        //                 : round3nodes;

        //     // Handle each case
        //     if (node is Key key)
        //     {
        //         if (roundNodes.Contains(node))
        //             key.ShowKey();
        //         else
        //             key.HideKey();

        //         continue;
        //     }

        //     if (node is Control control)
        //     {
        //         if (roundNodes.Contains(node))
        //         {
        //             control.Show();
        //             control.ProcessMode = ProcessModeEnum.Inherit;
        //         }
        //         else
        //         {
        //             control.Hide();
        //             control.ProcessMode = ProcessModeEnum.Disabled;
        //         }
        //         continue;
        //     }
        // }

        EmitSignal(SignalName.OnRoundStart, Round);
    }

    public void SelectSize(int size)
    {
        CurrentSize = size;
        _player.SwitchMovementData(size);
        _player.StateMachine.TransitionToState("walk");
        _hud.SetSizeButtonDisabled((HUD.ButtonSelector)size, true);
    }

    public void TogglePause()
    {
        if (GetTree().Paused)
            UnPause();
        else
            Pause();
    }

    public void Pause()
    {
        GetTree().Paused = true;
        _hud.ShowPauseMenu();
    }

    public void UnPause()
    {
        GetTree().Paused = false;
        _hud.HidePauseMenu();
    }
}
