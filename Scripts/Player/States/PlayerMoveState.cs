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

        // TODO: Implement a timer for how long the jump key has been held
        //      This would make it so the slow gravity can't be held the entire time.
        // Handle gravity
        bool applyGravity = true;
        float gravity = _controller.jumpKey ? md.GravityUp : _controller.MovementData.GravityDown;
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

        // Handle jumping
        if (_jumpKeyBuffer > 0 && _controller.IsOnFloor())
        {
            vel.Y = md.JumpVelocity;
            _jumpKeyBuffer = 0;
        }

        // Handle horizontal movementData
        float acc = _controller.IsOnFloor()
            ? md.HorizontalAcceleration
            : (md.HorizontalAcceleration * 0.5f); // TODO: Move hard coded value to movementData resource

        vel.X = Mathf.MoveToward(vel.X, _controller.movementInput.X * md.MaxSpeed, acc * dt);

        // Update Position
        _controller.Velocity = vel;
        bool collided = _controller.MoveAndSlide();

        if (collided)
            HandleCollisions(md, vel, dt);

        HandleSprite();
    }

    private void HandleCollisions(MovementData md, Vector2 vel, float dt)
    {
        for (int i = 0; i < _controller.GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = _controller.GetSlideCollision(i);

            if (md.CanPush && collision.GetCollider() is Pushable pushable)
            {
                Vector2 normal = collision.GetNormal();
                if (Mathf.Abs(normal.X) < Mathf.Abs(normal.Y))
                    continue;

                Vector2 push = vel * dt;
                push.Y = 0;
                pushable.MoveAndCollide(push);
            }

            // Test to drop through one way collisions
            if (_controller.movementInput.Y > 0.5f && _controller.IsOnFloor())
            {
                bool canDropThrough = false;
                if (collision.GetColliderShape() is CollisionShape2D shape && shape.OneWayCollision)
                    canDropThrough = true;

                if (!canDropThrough && collision.GetCollider() is TileMap tileMap)
                {
                    float xDelta = Mathf.Sign(collision.GetPosition().X - _controller.GlobalPosition.X);
                    Vector2I coord = tileMap.LocalToMap(_controller.GlobalPosition + Vector2.Down);
                    TileData data = tileMap.GetCellTileData(0, coord);
                    if (data == null)
                    {
                        int offset = Mathf.RoundToInt(xDelta);
                        coord.X += offset;
                        data = tileMap.GetCellTileData(0, coord);
                        if (data == null)
                        {
                            coord.X -= offset * 2;
                            data = tileMap.GetCellTileData(0, coord);
                        }
                    }

                    if (data != null)
                        canDropThrough = data.IsCollisionPolygonOneWay(0, 0);
                }

                if (canDropThrough)
                {
                    _controller.Position += Vector2.Down;
                }
            }
        }
    }

    private void HandleSprite()
    {
        _controller.animatedSprite2D.FlipH = !_controller.facingRight;
        if (_controller.IsOnFloor())
        {
            const float DEAD_ZONE = 0.1f;
            float hInput = Mathf.Abs(_controller.movementInput.X);
            float vInput = _controller.movementInput.Y;

            if (hInput <= DEAD_ZONE)
            {
                if (vInput < -DEAD_ZONE)
                    _controller.animatedSprite2D.Play("look_up");
                else if (vInput > DEAD_ZONE)
                    _controller.animatedSprite2D.Play("look_down");
                else if (_controller.animatedSprite2D.Animation != "idle")
                    _controller.animatedSprite2D.Play("idle");
            }
            else if (hInput > DEAD_ZONE && _controller.animatedSprite2D.Animation != "walk")
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
