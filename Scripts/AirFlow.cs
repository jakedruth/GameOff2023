using Godot;
using System;

public partial class AirFlow : Area2D
{
    [Export] private float _launchSpeed;

    public override void _Ready()
    {
        base._Ready();
        GetChild<GpuParticles2D>(2).Emitting = true;
        GetChild<AnimatedSprite2D>(1).Play("idle");
    }

    public void _on_body_entered(Node2D body)
    {
        if (body is PlayerController player)
        {
            Console.WriteLine("Player has entered");
            player.isInFan = true;
            if (player.MovementData.FanOverrideYSpeed)
            {
                Vector2 vel = player.Velocity;
                float deltaY = Mathf.Abs(player.Position.Y - Position.Y);
                float k = Mathf.Clamp(1 - deltaY / 80f, 0, 1);
                vel.Y = _launchSpeed * k;
                player.Velocity = vel;
            }
        }
    }

    public void _on_body_exited(Node2D body)
    {
        if (body is PlayerController player)
        {
            Console.WriteLine("Player has exited");
            player.isInFan = false;
        }
    }
}
