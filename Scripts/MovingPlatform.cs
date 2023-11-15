using Godot;
using System;

public partial class MovingPlatform : AnimatableBody2D
{
    private Path2D _path;

    [Export] public float speed;
    [Export] public float waitAtWayPointTime;
    [Export] public bool loop;
    private int _index;
    private bool _reverse;
    private float _timer;

    public override void _Ready()
    {
        SyncToPhysics = false;
        _path = GetParent<Path2D>();
        if (_path == null)
            throw new Exception("Could not get Path2D Node from parent. Make sure the parent is of type Path2D");
        Vector2 pos = _path.Curve.GetPointPosition(0);
        Position = pos;
        SyncToPhysics = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_path == null)
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
}
