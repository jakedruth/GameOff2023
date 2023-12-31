using Godot;
using Microsoft.VisualBasic;
using System;

public partial class FallingBlock : AnimatableBody2D
{
    public enum BlockState
    {
        NORMAL,
        WIGGLE,
        FALLING,
        DISAPEARING
    }
    private Sprite2D _sprite2D;
    private Vector2 _spritePos;

    private BlockState _blockState;
    private Vector2 _startPosition;
    [Export] public double wiggleTime;
    [Export] public int wiggleCount;
    private double _timer;
    [Export] public float fallDistance;
    [Export] public float fallSpeed;
    [Export] public double disapearTime;

    private PlayerController _playerController;

    public override void _Ready()
    {
        base._Ready();
        _sprite2D = GetChild<Sprite2D>(1);
        _spritePos = _sprite2D.Position;
        _startPosition = Position;
    }

    public void StartWiggle()
    {
        _blockState = BlockState.WIGGLE;
        _timer = wiggleTime;
    }

    public void StartFalling()
    {
        _blockState = BlockState.FALLING;
        _sprite2D.Position = _spritePos = new Vector2(0, 4);
    }

    public void StartDisapearing()
    {
        _blockState = BlockState.DISAPEARING;
        CollisionLayer = 0;
        _timer = disapearTime;
    }

    public void Reset()
    {
        Position = _startPosition;
        _playerController = null;
        _blockState = BlockState.NORMAL;
    }

    public void _on_area_2d_body_entered(Node2D body)
    {
        if (body is PlayerController player)
        {
            _playerController = player;
        }
    }

    public void _on_area_2d_body_exited(Node2D body)
    {
        if (body is PlayerController)
        {
            _playerController = null;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        switch (_blockState)
        {
            case BlockState.NORMAL:
                if (_playerController != null)
                {
                    int count = _playerController.GetSlideCollisionCount();
                    for (int i = 0; i < count; i++)
                    {
                        KinematicCollision2D collision2D = _playerController.GetSlideCollision(i);
                        var collider = collision2D.GetCollider();
                        if (collider == this)
                        {
                            StartWiggle();
                        }
                    }
                }
                break;
            case BlockState.WIGGLE:
                // Handle wiggling
                _timer -= delta;
                _spritePos.X = (float)Mathf.Sin(2.0 * wiggleCount * Mathf.Pi * _timer / wiggleTime);
                _sprite2D.Position = _spritePos;
                if (_timer <= 0)
                    StartFalling();

                break;
            case BlockState.FALLING:
                Vector2 step = Vector2.Down * fallSpeed * (float)delta;
                MoveAndCollide(step);
                if (Position.Y - _startPosition.Y >= fallDistance)
                    StartDisapearing();
                break;
            case BlockState.DISAPEARING:
                _timer -= delta;
                float t = 1 - (float)(_timer / disapearTime);
                t = Mathf.Clamp(t, 0, 1);
                Scale = Vector2.One.Lerp(Vector2.Zero, t);
                if (t == 0)
                    Free();
                break;
        }
    }
}
