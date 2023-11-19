using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class MovingPlatform : AnimatableBody2D, IInteractable
{
    private Path2D _path;
    private Sprite2D _spriteL;
    private Sprite2D _spriteR;

    [Export] public float speed;
    [Export] public float waitAtWayPointTime;
    [Export] public bool loop;
    [Export] public Color onColor;
    [Export] public Color offColor;

    private int _index;
    private bool _reverse;
    private float _timer;

    [Export] public bool CurrentState { get; set; }

    public override void _Ready()
    {
        SyncToPhysics = false;

        _path = GetParent<Path2D>();
        if (_path == null)
            throw new Exception("Could not get Path2D Node from parent. Make sure the parent is of type Path2D");

        _spriteL = GetChild<Sprite2D>(1);
        _spriteR = GetChild<Sprite2D>(2);

        Vector2 pos = _path.Curve.GetPointPosition(0);
        Position = pos;

        SyncToPhysics = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CurrentState || _path == null)
            return;

        float dt = (float)delta;

        if (_timer > 0)
        {
            _timer -= dt;
            return;
        }

        Vector2 target = _path.Curve.GetPointPosition(_index);
        Position = Position.MoveToward(target, speed * dt);

        if (target == Position)
        {
            _timer = waitAtWayPointTime;
            _index += _reverse ? -1 : 1;

            if (_index >= _path.Curve.PointCount)
            {
                if (loop)
                    _index = 0;
                else
                {
                    _index = _path.Curve.PointCount - 1;
                    _reverse = true;
                }
            }
            else if (_index < 0)
            {
                _index = 0;
                _reverse = false;
            }
        }
    }

    void IInteractable.OnStateChanged(bool newState)
    {
        _spriteL.Modulate = _spriteR.Modulate = newState ? onColor : offColor;
    }
}
