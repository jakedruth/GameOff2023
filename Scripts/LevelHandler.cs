using Godot;
using System;

public partial class LevelHandler : Node2D
{
    public int Round { get; private set; }
    public int CurrentSize { get; private set; }

    private PlayerController _player;
    private Vector2 _playerSpawnPoint;

    public override void _Ready()
    {
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
            // TODO: Handle completing last Round
            return;
        }

        SetUpRound();
    }

    public void RedoRound()
    {
        // SetUpRound();
    }

    private void SetUpRound()
    {
        _player.GlobalPosition = _playerSpawnPoint;
    }

    public void SelectSize(int size)
    {
        GD.Print($"LevelHandler.SelectSize() : {size}");
        CurrentSize = size;
        _player.SwitchMovementData(size);
        _player.StateMachine.TransitionToState("walk");
    }
}
