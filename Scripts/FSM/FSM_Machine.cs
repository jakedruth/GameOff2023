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
            state.TransitionToState += TransitionToState;
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

        public void TransitionToState(string targetState)
        {
            FSM_State nextState = _states[targetState.ToLower()];
            TransitionToState(nextState);
        }

        public void TransitionToState(FSM_State targetState)
        {
            if (curretnState == targetState)
                return;

            curretnState?.OnExit();
            targetState?.OnEnter();
            curretnState = targetState;
        }
    }

    public abstract partial class FSM_State
    {
        public FSM_Machine Machine { get; private set; }
        public abstract string StateName { get; }

        public FSM_State(FSM_Machine machine)
        {
            Machine = machine;
            Machine.AddState(this);
        }

        public Action<string> TransitionToState;

        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Process(float dt);
        public abstract void PhysicsProcess(float dt);
    }
}


