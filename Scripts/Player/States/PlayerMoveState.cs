using System;
using FSM;
using Godot;

[GlobalClass]
public partial class PlayerMoveState : FSM_State
{
    private PlayerController _controller;
    private float _jumpKeyBuffer;
    private float maxY;

    public override void OnEnter()
    {
        _controller = Machine.Owner as PlayerController;
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
        // Get current velocity
        Vector2 vel = _controller.Velocity;
        MovementData md = _controller.MovementData;

        // Handle gravity
        bool applyGravity = true;
        float gravity = vel.Y >= 0 ? _controller.MovementData.GravityDown : md.GravityUp;
        if (_controller.isInFan)
        {
            if (md.FanOverrideYSpeed)
            {
                vel.Y = Mathf.MoveToward(vel.Y, md.FanOverrideYSpeedAmount, gravity * dt);
                applyGravity = false;
            }
            else
            {
                gravity *= md.GravityFanMultiplier;
            }
        }

        if (applyGravity)
        {
            if (!_controller.IsOnFloor())
                vel.Y += gravity * dt;
            else
                vel.Y = gravity * dt;
        }

        // TODO: Implement a variable jump height
        // Handle jumping
        if (_jumpKeyBuffer > 0 && _controller.IsOnFloor())
            vel.Y = md.JumpVelocity;

        // Handle horizontal movementData
        vel.X = Mathf.MoveToward(vel.X, _controller.movementInput.X * md.MaxSpeed, md.HorizontalAcceleration * dt);

        // Update Position
        _controller.Velocity = vel;
        bool collided = _controller.MoveAndSlide();
        if (collided)
        {
            for (int i = 0; i < _controller.GetSlideCollisionCount(); i++)
            {
                var collision = _controller.GetSlideCollision(i);
                if (collision.GetCollider() is Pushable pushable)
                {
                    Vector2 normal = collision.GetNormal();
                    if (Mathf.Abs(normal.X) >= Mathf.Abs(normal.Y))
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
            float MINSPEED = _controller.MovementData.HorizontalAcceleration * AVG_DT * 1.1f;
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
            if (_controller.Velocity.Y <= 10)
                _controller.animatedSprite2D.Play("midair_up");
            else
                _controller.animatedSprite2D.Play("midair_down");
        }
    }
}
