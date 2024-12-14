using System;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        private IState CurrentState { get; set; }

        public CubeFallingState CubeFallingState;

        public StateMachine(GridManager gridManager)
        {
            CubeFallingState = new CubeFallingState(gridManager);
        }
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
        }
        public void Update()
        {
            CurrentState?.Update();
        }
    }
}