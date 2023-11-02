using FSM;

public class IdleState : FSM_Machine<PlayerController>.FSM_State
{
    public IdleState(FSM_Machine<PlayerController> machine) : base(machine)
    {
    }

    public override string Name { get { return "idle"; } }
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void PhysicsProcess(double dt)
    {
        throw new System.NotImplementedException();
    }

    public override void Process(double dt)
    {
        throw new System.NotImplementedException();
    }
}
