using Godot;
using System;

public partial class Pushable : StaticBody2D
{
    private Node2D _imgHolder;
    private RayCast2D _rayCast2D;

    [Export] float velocity;

    public override void _Ready()
    {
        _imgHolder = GetChild<Node2D>(1);
        _rayCast2D = GetChild<RayCast2D>(2);
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        // Handle Physics
        bool rayHit = _rayCast2D.IsColliding();

        Vector2 step = Vector2.Down * velocity * dt;
        if (rayHit)
        {
            Vector2 dir = _rayCast2D.GetCollisionNormal().Normalized();
            dir.Y = 0;
            step += dir * velocity * dt;
        }

        MoveAndCollide(step);

        // Handle Sprite Rotation
        Vector2 normal = Vector2.Up;
        if (rayHit)
        {
            normal = _rayCast2D.GetCollisionNormal();
        }

        float target = normal.Angle() + (Mathf.Pi / 2f);
        _imgHolder.Rotation = (float)Mathf.Lerp(_imgHolder.Rotation, target, 1 - Mathf.Exp(-20 * delta));
    }

    private void HandleSlide()
    {

    }
}
