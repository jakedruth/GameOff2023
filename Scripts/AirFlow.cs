using Godot;
using System;

public partial class AirFlow : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        GetChild<GpuParticles2D>(2).Emitting = true;
    }

    public void _on_body_entered(Node2D body)
    {
        if (body is PlayerController player)
        {
            Console.WriteLine("Player has entered");
            player.isInFan = true;
            // if (player.StateMachine.curretnState is PlayerMoveState state)
            // {
            //     if (state.MovementData.FanOverrideYSpeed)
            //     {
            //         Vector2 vel = player.Velocity;
            //         vel.Y = -10;
            //         player.Velocity = vel;
            //     }
            // }
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
