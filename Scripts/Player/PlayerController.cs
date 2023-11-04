using Godot;
using System;
using FSM;

public partial class PlayerController : CharacterBody2D
{
    public AnimatedSprite2D animatedSprite2D { get; private set; }

    private FSM_Machine _stateMachine;

    [Export] FSM_State[] _states;

    public Vector2 movementInput;
    public bool jumpKey;
    public bool onJumpKey;
    public bool facingRight;

    public override void _Ready()
    {
        animatedSprite2D = GetChild<AnimatedSprite2D>(1);
        facingRight = animatedSprite2D.FlipH;

        _stateMachine = new FSM_Machine(this);
        for (int i = 0; i < _states.Length; i++)
        {
            _states[i].Init(_stateMachine);
        }

        _stateMachine.Ready(_states[0].StateName);
    }

    public override void _Process(double delta)
    {
        float dt = (float)delta;

        // Get input
        movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        jumpKey = Input.IsActionPressed("jump");
        onJumpKey = Input.IsActionJustPressed("jump");

        if (Input.IsKeyLabelPressed(Key.Kp1))
            _stateMachine.curretnState.Transition(_stateMachine.curretnState, "Small");

        if (Input.IsKeyLabelPressed(Key.Kp2))
            _stateMachine.curretnState.Transition(_stateMachine.curretnState, "Normal");

        if (Input.IsKeyLabelPressed(Key.Kp3))
            _stateMachine.curretnState.Transition(_stateMachine.curretnState, "Large");


        // Update the state machine
        _stateMachine.Process(dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        // Update the state machine
        _stateMachine.PhysicsProcess(dt);
    }
}
