using Godot;
using System;
using FSM;

public partial class PlayerController : Node
{
    public FSM_Machine<PlayerController> stateMachine;

    public override void _Ready()
    {
        stateMachine = new FSM_Machine<PlayerController>(this);

        IdleState idle = new IdleState(stateMachine);

        stateMachine.Ready(idle.Name);
    }
}
