using CodeBase.StateMachine;
using CodeBase.StateMachine.States;
using UnityEngine;
using Zenject;

namespace CodeBase
{
    public class Bootstrap : MonoBehaviour
    {
        private GameStateMachine gameStateMachine;

        [Inject]
        public void Construct(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        private void Awake() =>
            gameStateMachine.Enter<BootstrapState>();
    }
}