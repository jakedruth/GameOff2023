using Godot;
using System;

public partial class PlayerMovementData : Resource
{
    [Export] public float maxSpeed;
    [Export] public float acceleration;
    [Export] public float jumpHeight;
    [Export] public float skewAmountOnVelocity;
}
