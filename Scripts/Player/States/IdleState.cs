using System;
using FSM;
using Godot;

public partial class IdleState : FSM_State
{
    public IdleState(FSM_Machine machine) : base(machine)
    {
    }

    public override string StateName => "idle";

    public override void OnEnter() { }

    public override void OnExit() { }

    public override void PhysicsProcess(float dt) { }

    public override void Process(float dt) { }
}