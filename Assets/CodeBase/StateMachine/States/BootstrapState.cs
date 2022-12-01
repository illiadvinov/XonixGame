using CodeBase.Enemy;
using CodeBase.Grid;
using Zenject;

namespace CodeBase.StateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly GridManager gridManager;
        private readonly EnemyPool enemyPool;

        [Inject]
        public BootstrapState(
            GameStateMachine gameStateMachine,
            GridManager gridManager,
            EnemyPool enemyPool)
        {
            this.gameStateMachine = gameStateMachine;
            this.gridManager = gridManager;
            this.enemyPool = enemyPool;
        }

        public void Enter()
        {
            gridManager.Initialize();
            enemyPool.Initialize();
            gameStateMachine.Enter<LevelState>();
        }

        public void Exit()
        {
        }
    }
}