using Godot;
using System;
using System.Collections.Generic;

namespace FSM
{
    public partial class FSM_Machine
    {
        public object Owner { get; private set; }
        public Dictionary<string, FSM_State> _states;
        public FSM_State curretnState;

        public FSM_Machine(object Owner)
        {
            this.Owner = Owner;
            _states = new Dictionary<string, FSM_State>();
        }

        public void AddState(FSM_State state)
        {
            _states.Add(state.StateName.ToLower(), state);
            state.Transition += OnStateTransition;
        }

        public void Ready(string startState)
        {
            curretnState = _states[startState.ToLower()];
            curretnState?.OnEnter();
        }

        public void Process(float dt)
        {
            curretnState?.Process(dt);
        }

        public void PhysicsProcess(float dt)
        {
            curretnState?.PhysicsProcess(dt);
        }

        private void OnStateTransition(FSM_State sender, string targetState)
        {
            if (sender != curretnState)
                return;

            FSM_State nextState = _states[targetState.ToLower()];

            if (curretnState == nextState)
                return;

            curretnState?.OnExit();
            nextState?.OnEnter();
            curretnState = nextState;
        }
    }

    public abstract partial class FSM_State : Resource
    {
        public FSM_Machine Machine { get; private set; }
        [Export] public virtual string StateName { get; protected set; }

        public void Init(FSM_Machine machine)
        {
            Machine = machine;
            Machine.AddState(this);
        }

        public Action<FSM_State, string> Transition;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Process(float dt);
        public abstract void PhysicsProcess(float dt);
    }
}


