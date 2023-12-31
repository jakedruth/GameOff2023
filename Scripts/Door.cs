using Godot;
using System;

public partial class Door : Area2D
{
    private PlayerController _playerController;
    private bool _isOpen;

    public void _on_body_entered(Node2D body)
    {
        if (body is PlayerController player)
            _playerController = player;
    }

    public void _on_body_exited(Node2D body)
    {
        if (body is PlayerController)
            _playerController = null;
    }

    public override void _Process(double delta)
    {
        if (_playerController == null || !_isOpen)
            return;

        if (_playerController.IsOnFloor() && _playerController.movementInput.Y <= -0.5f)
        {
            CloseDoor();
            LevelHandler levelHandler = GetTree().GetFirstNodeInGroup("LevelHandler") as LevelHandler;
            levelHandler.CompletedRound();
        }
    }

    public void OpenDoor()
    {
        if (_isOpen)
            return;

        _isOpen = true;
        Rect2 rect = new Rect2(0, 24, 24, 24);
        GetChild<Sprite2D>(1).RegionRect = rect;
    }

    public void CloseDoor()
    {
        if (!_isOpen)
            return;

        _isOpen = false;
        Rect2 rect = new Rect2(0, 0, 24, 24);
        GetChild<Sprite2D>(1).RegionRect = rect;
    }
}
