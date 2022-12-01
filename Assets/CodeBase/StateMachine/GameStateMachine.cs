using System;
using System.Collections.Generic;
using CodeBase.StateMachine.States;
using Zenject;

namespace CodeBase.StateMachine
{
    public class GameStateMachine
    {
        private Dictionary<Type, IState> states;
        private IState currentState;

        [Inject]
        public void Construct(
            [Inject(Id = "BootstrapState")] IState boostrapState,
            [Inject(Id = "LevelState")] IState LevelState)
        {
            states = new Dictionary<Type, IState>
            {
                [typeof(BootstrapState)] = boostrapState,
                [typeof(LevelState)] = LevelState
            };
        }

        public void Enter<TState>() where TState : IState
        {
            currentState?.Exit();
            IState state = states[typeof(TState)];
            currentState = state;
            state.Enter();
        }
    }
}