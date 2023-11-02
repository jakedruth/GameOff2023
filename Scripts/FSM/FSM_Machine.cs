using Godot;
using System;
using System.Collections.Generic;

namespace FSM
{
    public class FSM_Machine<T>
    {
        public T Owner { get; private set; }
        public Dictionary<string, FSM_State> _states;
        private FSM_State _curretnState;

        public FSM_Machine(T Owner)
        {
            this.Owner = Owner;
            _states = new Dictionary<string, FSM_State>();
        }

        protected void AddState(FSM_State state)
        {
            _states.Add(state.Name.ToLower(), state);
            state.Transition += OnStateTransition;
        }

        public void Ready(string startState)
        {
            _curretnState = _states[startState.ToLower()];
            _curretnState?.OnEnter();
        }

        public void Process(double dt)
        {
            _curretnState?.Process(dt);
        }

        public void PhysicsProcess(double dt)
        {
            _curretnState?.PhysicsProcess(dt);
        }

        public void OnStateTransition(FSM_State sender, string targetState)
        {
            if (sender != _curretnState)
                return;

            FSM_State nextState = _states[targetState];
            if (nextState == null)
                return;

            _curretnState.OnExit();
            nextState.OnEnter();
            _curretnState = nextState;
        }

        public abstract class FSM_State
        {
            public FSM_Machine<T> Machine { get; private set; }
            public abstract string Name { get; }
            public FSM_State(FSM_Machine<T> machine)
            {
                Machine = machine;
                Machine.AddState(this);
            }

            public Action<FSM_State, string> Transition;

            public abstract void OnEnter();
            public abstract void OnExit();
            public abstract void Process(double dt);
            public abstract void PhysicsProcess(double dt);
        }
    }
}
