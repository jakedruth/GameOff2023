using Godot;
using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

public partial class PressurePlate : Area2D
{
    [ExtenderProvidedProperty] public Toggle toggle;

    public override void _Ready()
    {
    }

}

[Serializable]
[ExtenderProvidedProperty]
public class Toggle
{
    [Export] public bool currentState;
}

