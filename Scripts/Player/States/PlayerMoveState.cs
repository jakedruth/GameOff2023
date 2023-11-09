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

        // TODO: Implement a variable jump height
        // Handle jumping
        if (_jumpKeyBuffer > 0 && _controller.IsOnFloor())
            vel.Y = _jumpForce;

        // Handle horizontal movementData
        vel.X = Mathf.MoveToward(vel.X, _controller.movementInput.X * _movementData.maxSpeed, _movementData.horizontalAcceleration * dt);

        // TODO: Fix how I implemented pushing blocks
        // Update Position
        _controller.Velocity = vel;
        bool collided = _controller.MoveAndSlide();
        if (collided)
        {
            float force = 20;
            for (int i = 0; i < _controller.GetSlideCollisionCount(); i++)
            {
                var collision = _controller.GetSlideCollision(i);
                if (collision.GetCollider() is RigidBody2D other)
                {
                    if (other.IsInGroup("Box"))
                    {
                        other.ApplyCentralImpulse(collision.GetNormal() * -force);
                    }
                    // other.ApplyForce(collision.GetNormal() * -force);
                }
                if (collision.GetCollider() is Pushable pushable)
                {
                    Vector2 normal = collision.GetNormal();
                    float dot = Vector2.Up.Dot(normal);
                    if (Mathf.Abs(dot) <= 0.1f)
                    {
                        Vector2 push = vel * dt;
                        push.Y = 0;
                        pushable.MoveAndCollide(push);
                    }
                }
            }
        }
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
            _controller.animatedSprite2D.Play("jump");
        }
    }
}
