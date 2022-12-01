using CodeBase.Camera;
using CodeBase.Coroutine;
using CodeBase.Enemy;
using CodeBase.Grid;
using CodeBase.Player;
using CodeBase.StateMachine;
using CodeBase.StateMachine.States;
using CodeBase.Tile;
using CodeBase.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Transform enemyParent;
        [SerializeField] private Transform tileParent;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Transform activeEnemiesContainer;
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text paintedPercentageText;
        [SerializeField] private TMP_Text livesLeftText;
        [SerializeField] private PauseButton pauseButton;
        [SerializeField] private GameObject player;


        public override void InstallBindings()
        {
            BindServices();
            BindContainers();
            BindStates();
            BindStateMachine();
            BindMonoBehaviours();
            BindTexts();
            BindGameObjects();
        }

        private void BindGameObjects()
        {
            Container.Bind<GameObject>().WithId("Player").FromInstance(player);
        }

        private void BindTexts()
        {
            Container.Bind<TMP_Text>().WithId("Timer").FromInstance(timerText);
            Container.Bind<TMP_Text>().WithId("PaintedPercentage").FromInstance(paintedPercentageText);
            Container.Bind<TMP_Text>().WithId("LivesLeft").FromInstance(livesLeftText);
        }

        private void BindMonoBehaviours()
        {
            Container.Bind<PlayerMovement>().FromInstance(playerMovement);
            Container.Bind<PauseButton>().FromInstance(pauseButton);
        }

        private void BindStateMachine()
        {
            Container.Bind<GameStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<IState>().WithId("BootstrapState").To<BootstrapState>().AsSingle();
            Container.Bind<IState>().WithId("LevelState").To<LevelState>().AsSingle();
        }

        private void BindContainers()
        {
            Container.Bind<Transform>().WithId("EnemyParent").FromInstance(enemyParent);
            Container.Bind<Transform>().WithId("TileParent").FromInstance(tileParent);
            Container.Bind<Transform>().WithId("ActiveEnemiesContainer").FromInstance(activeEnemiesContainer);
        }

        private void BindServices()
        {
            Container.Bind<CameraService>().AsSingle();
            Container.Bind<TileManager>().AsSingle();
            Container.Bind<EnemyPool>().AsSingle();
            Container.Bind<GridManager>().AsSingle();
            Container.Bind<EnemyManager>().AsSingle();
            Container.Bind<PlayerHealth>().AsSingle();
            Container.Bind<InputSystem.InputSystem>().AsSingle();
            Container.Bind<TimerController>().AsSingle();
            Container.Bind<PlayerProgress>().AsSingle();
            Container.Bind<ICoroutineRunner>().FromInstance(coroutineRunner).AsSingle();
        }
    }
}