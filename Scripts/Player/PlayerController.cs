using Godot;
using System;
using FSM;

public partial class PlayerController : CharacterBody2D
{
    public AnimatedSprite2D animatedSprite2D { get; private set; }

    public FSM_Machine StateMachine { get; private set; }

    [Export] FSM_State[] _states;

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

        StateMachine.Ready(_states[0].StateName);
    }

    public override void _Process(double delta)
    {
        float dt = (float)delta;

        // Get input
        movementInput = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        jumpKey = Input.IsActionPressed("jump");
        onJumpKey = Input.IsActionJustPressed("jump");

        // TODO: Do I want to have multiple, duplicate states for small, medium, large?
        //  Or should I have only multiple move data resources that get switched around? I like this option better personally.

        if (Input.IsKeyLabelPressed(Key.Kp1))
            StateMachine.curretnState.Transition(StateMachine.curretnState, "Small");

        if (Input.IsKeyLabelPressed(Key.Kp2))
            StateMachine.curretnState.Transition(StateMachine.curretnState, "Normal");

        if (Input.IsKeyLabelPressed(Key.Kp3))
            StateMachine.curretnState.Transition(StateMachine.curretnState, "Large");


        // Update the state machine
        StateMachine.Process(dt);
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        // Update the state machine
        StateMachine.PhysicsProcess(dt);
    }
}
