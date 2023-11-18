using Godot;
using System;
using FSM;

public partial class PlayerController : CharacterBody2D
{
    public AnimatedSprite2D animatedSprite2D { get; private set; }

    public FSM_Machine StateMachine { get; private set; }

    [Export] FSM_State[] _states;
    [Export] MovementData[] _movementDatas;
    public MovementData MovementData { get; private set; }

    public Vector2 movementInput;
    public bool jumpKey;
    public bool onJumpKey;
    public bool facingRight;
    public bool isInFan;

    public override void _Ready()
    {
        animatedSprite2D = GetChild<AnimatedSprite2D>(1);
        facingRight = animatedSprite2D.FlipH;

        StateMachine = new FSM_Machine(this);
        for (int i = 0; i < _states.Length; i++)
        {
            _states[i].Init(StateMachine);
        }

        SwitchMovementData(1);
        StateMachine.Ready(_states[0].StateName);
    }

    public override void _Process(double delta)
    {
        float dt = (float)delta;

        // Get input
        movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        jumpKey = Input.IsActionPressed("jump");
        onJumpKey = Input.IsActionJustPressed("jump");

        // Debug to change sizes
        if (Input.IsKeyLabelPressed(Key.Kp1))
            SwitchMovementData(0);

        if (Input.IsKeyLabelPressed(Key.Kp2))
            SwitchMovementData(1);

        if (Input.IsKeyLabelPressed(Key.Kp3))
            SwitchMovementData(2);

        if (Input.IsKeyLabelPressed(Key.R))
        {
            GetNode<SceneManager>("/root/SceneManager").GoToScene(0);
        }

        // Update the state machine
        StateMachine.Process(dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        // Update the state machine
        StateMachine.PhysicsProcess(dt);
    }

    public void SwitchMovementData(int index)
    {
        MovementData = _movementDatas[index];
        Scale = Vector2.One * MovementData.PlayerScale;
    }
}
