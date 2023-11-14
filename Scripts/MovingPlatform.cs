using Godot;
using System;

[Tool]
public partial class MovingPlatform : AnimatableBody2D
{
    [Export] public float speed;
    [Export] public float waitAtWayPointTime;
    [Export] public bool loop;
    private int _dir = 1;

    public override void _PhysicsProcess(double delta)
    {

    }

    public override string[] _GetConfigurationWarnings()
    {
        Path2D parent = GetParentOrNull<Path2D>();
        if (parent == null)
        {
            return new string[] { "Parent must be of type Path2D" };
        }
        return base._GetConfigurationWarnings();
    }
}
