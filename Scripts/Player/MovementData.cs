using Godot;
using System;

[GlobalClass]
public partial class MovementData : Resource
{
    [Export] public float maxSpeed;
    [Export] public float horizontalAcceleration;
    [Export] public float jumpHeight;
    [Export] public float gravityUp;
    [Export] public float gravityDown;


}
