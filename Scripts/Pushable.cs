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
        KinematicCollision2D collision = MoveAndCollide(step);
        if (collision != null)
        {
            Vector2 normal = collision.GetNormal();
            if (normal.Dot(Vector2.Up) < 0.9f)
            {
                Vector2 slide = step.Slide(normal);
                MoveAndCollide(slide);
            }
            else
            {
                if (rayHit)
                {
                    Vector2 dir = _rayCast2D.GetCollisionNormal();
                    dir.Y = 0;
                    dir = dir.Normalized();
                    step += dir * velocity * dt;
                    MoveAndCollide(step);
                }
            }
        }

        // Handle Sprite Rotation
        Vector2 spriteFacing = Vector2.Up;
        if (rayHit)
        {
            spriteFacing = _rayCast2D.GetCollisionNormal();
        }

        float target = spriteFacing.Angle() + (Mathf.Pi / 2f);
        _imgHolder.Rotation = (float)Mathf.Lerp(_imgHolder.Rotation, target, 1 - Mathf.Exp(-20 * delta));
        Vector2 offset = Vector2.Zero;
        offset.Y = -Mathf.Sin(_imgHolder.Rotation);
        _imgHolder.Position = offset;
    }

    // TODO: Remove this function. This is a test to see if the action
    private void HandleSlide()
    {

    }
}
