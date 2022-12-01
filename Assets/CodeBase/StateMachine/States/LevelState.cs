using CodeBase.Enemy;
using CodeBase.Grid;
using CodeBase.Player;
using CodeBase.Tile;
using CodeBase.UI;
using UnityEngine;
using Zenject;

namespace CodeBase.StateMachine.States
{
    public class LevelState : IState
    {
        private readonly EnemyManager enemyManager;
        private readonly InputSystem.InputSystem inputSystem;
        private readonly PlayerHealth playerHealth;
        private readonly PlayerMovement playerMovement;
        private readonly TimerController timerController;
        private readonly GridManager gridManager;
        private readonly TileManager tileManager;
        private readonly GameStateMachine gameStateMachine;
        private readonly GameObject player;
        private int level = 0;

        [Inject]
        public LevelState(
            EnemyManager enemyManager,
            InputSystem.InputSystem inputSystem,
            PlayerHealth playerHealth,
            PlayerMovement playerMovement,
            TimerController timerController,
            GridManager gridManager,
            TileManager tileManager,
            GameStateMachine gameStateMachine,
            [Inject(Id = "Player")] GameObject player)
        {
            this.enemyManager = enemyManager;
            this.inputSystem = inputSystem;
            this.playerHealth = playerHealth;
            this.playerMovement = playerMovement;
            this.timerController = timerController;
            this.gridManager = gridManager;
            this.tileManager = tileManager;
            this.gameStateMachine = gameStateMachine;
            this.player = player;
            timerController.OnTimerStop += RepeatLevel;
            playerHealth.OnHealthDecreased += RepeatLevel;
        }

        public void Enter()
        {
            level++;
            playerHealth.ResetHealth();
            enemyManager.SpawnEnemies(level);
            timerController.StartTimer();
            inputSystem.StartInput();
            enemyManager.StartEnemiesMovement();
            player.SetActive(true);
        }

        private void RepeatLevel()
        {
            level--;
            gameStateMachine.Enter<LevelState>();
        }

        public void Exit()
        {
            enemyManager.DisableEnemies();
            timerController.StopTimer();
            player.SetActive(false);
            inputSystem.StopInput();
            gridManager.Reset();
            tileManager.ResetPaintedTilesCount();
        }
    }
}