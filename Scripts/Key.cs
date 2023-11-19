using Godot;
using System;
using System.Collections.Generic;

public partial class Key : Area2D
{
    private Door _door;
    private QubicBezierCurve _curve;

    [Export] float p1Height;
    [Export] double timeToPlayer;
    private double _timer;
    private int _rotateDir;
    private float _startScale;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerController)
        {
            _door = GetTree().GetFirstNodeInGroup("Door") as Door;

            // Launch the key
            LaunchKey();

            // Disable collision
            CollisionMask = 0;
        }
    }

    public override void _Process(double delta)
    {
        if (_door == null || _curve == null)
            return;

        _timer += delta;
        float t = (float)Mathf.Clamp(_timer / timeToPlayer, 0, 1);

        //_curve.p3 = _door.Position;
        Position = _curve.Sample(t);
        Rotation = Mathf.Tau * t;
        float invT = 1 - t;
        Rotation = Mathf.Tau * _rotateDir * (1 + invT * invT * invT * invT);
        Scale = Vector2.One * _startScale * Mathf.Clamp(invT, 0.3f, 1);

        if (t >= 1)
        {
            // Open the door
            Door door = GetTree().GetFirstNodeInGroup("Door") as Door;
            door.OpenDoor();

            // Remove from the scene
            Free();
        }
    }

    private void LaunchKey()
    {
        _rotateDir = -Mathf.Sign(_door.Position.X - Position.X);
        _startScale = Scale.X;
        Vector2 p1 = GlobalPosition + Vector2.Up * p1Height;
        Vector2 p3 = _door.GlobalPosition + Vector2.Up * 8;
        _curve = new QubicBezierCurve(GlobalPosition, p1, p1, p3);
    }
}
