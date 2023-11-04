using System;
using FSM;
using Godot;

[GlobalClass]
public partial class PlayerMoveState : FSM_State
{
    private PlayerController _controller;

    [Export] MovementData _movementData;
    [Export] float _playerScale;
    private float _jumpKeyBuffer;
    private float _jumpForce;

    public override void OnEnter()
    {
        _controller = Machine.Owner as PlayerController;
        _jumpForce = -Mathf.Sqrt(2 * _movementData.jumpHeight * _movementData.gravityUp);
        _controller.Scale = Vector2.One * _playerScale;
    }

    public override void OnExit()
    {
    }

    public override void Process(float dt)
    {
        // Handle jump buffer
        if (_jumpKeyBuffer > 0)
            _jumpKeyBuffer -= dt;

        if (_controller.onJumpKey)
            _jumpKeyBuffer = 0.1f;

        // Update facing
        if (_controller.movementInput.X > 0 && !_controller.facingRight)
            _controller.facingRight = true;

        if (_controller.movementInput.X < 0 && _controller.facingRight)
            _controller.facingRight = false;
    }

    public override void PhysicsProcess(float dt)
    {
        Vector2 vel = _controller.Velocity;

        // Handle gravity
        if (!_controller.IsOnFloor())
            vel.Y += _movementData.gravityDown * dt;
        else
            vel.Y = _movementData.gravityDown * dt;

        // Handle jumping
        if (_jumpKeyBuffer > 0 && _controller.IsOnFloor())
            vel.Y = _jumpForce;

        // Handle horizontal movementData
        vel.X = Mathf.MoveToward(vel.X, _controller.movementInput.X * _movementData.maxSpeed, _movementData.horizontalAcceleration * dt);

        // Update Position
        _controller.Velocity = vel;
        _controller.MoveAndSlide();
        HandleSprite();
    }

    private void HandleSprite()
    {
        _controller.animatedSprite2D.FlipH = !_controller.facingRight;
        if (_controller.IsOnFloor())
        {
            const float AVG_DT = 0.0166f;
            float MINSPEED = _movementData.horizontalAcceleration * AVG_DT * 1.1f;
            float hSpeed = Mathf.Abs(_controller.Velocity.X);
            if (hSpeed < MINSPEED && _controller.animatedSprite2D.Animation != "idle")
            {
                _controller.animatedSprite2D.Play("idle");
            }
            else if (hSpeed >= MINSPEED && _controller.animatedSprite2D.Animation != "walk")
            {
                _controller.animatedSprite2D.Play("walk");
            }
        }
        else
        {
            Console.WriteLine("In The Air");
            _controller.animatedSprite2D.Play("jump");
        }
    }
}
