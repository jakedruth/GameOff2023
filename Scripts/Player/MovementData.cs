using Godot;
using System;

[GlobalClass]
public partial class MovementData : Resource
{
    //TODO: Convert each field to a getter and setter
    // The end goal is to have a field called jump velocity that gets set automatically when jumpheight or gravity is changed
    [Export] public float maxSpeed;
    [Export] public float horizontalAcceleration;
    [Export] public float jumpHeight;
    [Export] public float gravityUp;
    [Export] public float gravityDown;
}
