using Godot;
using System;

[GlobalClass]
public partial class MovementData : Resource
{
    private float _playerScale;
    private float _maxSpeed;
    private float _horizontalAcceleration;
    private float _jumpHeight;
    private float _gravityUp;
    private float _gravityDown;
    private float _gravityFanMultiplier;
    private float _jumpVelocity;

    [Export] public float PlayerScale { get => _playerScale; private set => _playerScale = value; }
    [Export] public float MaxSpeed { get => _maxSpeed; private set => _maxSpeed = value; }
    [Export] public float HorizontalAcceleration { get => _horizontalAcceleration; private set => _horizontalAcceleration = value; }
    [Export] public float GravityUp { get => _gravityUp; private set => _gravityUp = value; }
    [Export] public float GravityDown { get => _gravityDown; private set => _gravityDown = value; }
    [Export] public float GravityFanMultiplier { get => _gravityFanMultiplier; private set => _gravityFanMultiplier = value; }
    [Export]
    public float JumpHeight
    {
        get { return _jumpHeight; }
        private set
        {
            _jumpHeight = value;
            _jumpVelocity = -Mathf.Sqrt(2 * _jumpHeight * _gravityUp);
        }
    }
    public float JumpVelocity { get { return _jumpVelocity; } }
}
