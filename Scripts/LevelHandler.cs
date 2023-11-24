using Godot;
using System;

public partial class LevelHandler : Node2D
{
    private HUD _hud;

    public int Round { get; private set; }
    public int CurrentSize { get; private set; }

    private PlayerController _player;
    private Vector2 _playerSpawnPoint;

    public override void _Ready()
    {
        _hud = GetChild<HUD>(0);
        _hud.DisplayCharacterSelect();

        _player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;
        if (_player == null)
            throw new Exception("Could not locate player ndoe");

        _playerSpawnPoint = _player.GlobalPosition;
        Round = 0;
        CurrentSize = -1;

        SetUpRound();
    }

    public void CompletedRound()
    {
        Round++;
        if (Round >= 3)
        {
            CompletedLevel();
            return;
        }

        SetUpRound();
    }

    private void CompletedLevel()
    {
        _player.StateMachine.TransitionToState("idle");
        _hud.DisplayLevelComplete();
    }

    public void RedoRound()
    {
        // SetUpRound();
    }

    private void SetUpRound()
    {
        _player.GlobalPosition = _playerSpawnPoint;
        _player.StateMachine.TransitionToState("idle");
        _hud.DisplayCharacterSelect();
    }

    public void SelectSize(int size)
    {
        CurrentSize = size;
        _player.SwitchMovementData(size);
        _player.StateMachine.TransitionToState("walk");
        _hud.SetSizeButtonDisabled((HUD.ButtonSelector)size, true);
    }
}
