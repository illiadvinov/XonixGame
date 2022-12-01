using System.Collections.Generic;
using CodeBase.Camera;
using CodeBase.Player;
using CodeBase.Tile;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyPool
    {
        private readonly Transform enemyParent;
        private readonly TileManager tileManager;
        private readonly CameraService cameraService;
        private readonly PlayerMovement playerMovement;
        private readonly PlayerHealth playerHealth;
        private readonly List<GameObject> enemiesWater;
        private readonly List<GameObject> enemiesEarth;
        private readonly EnemyFactory enemyFactory;

        [Inject]
        public EnemyPool(
            TileManager tileManager,
            CameraService cameraService,
            PlayerMovement playerMovement,
            PlayerHealth playerHealth,
            [Inject(Id = "EnemyParent")] Transform enemyParent)
        {
            this.enemyParent = enemyParent;
            this.tileManager = tileManager;
            this.cameraService = cameraService;
            this.playerMovement = playerMovement;
            this.playerHealth = playerHealth;
            enemyFactory = new EnemyFactory();
            enemiesWater = new List<GameObject>();
            enemiesEarth = new List<GameObject>();
        }

        public void Initialize()
        {
            for (int i = 0; i < 2; i++)
            {
                EnemyType enemyType = (EnemyType) i;
                for (int j = 0; j < 5; j++)
                {
                    var enemy = enemyFactory.LoadEnemy(enemyType, enemyParent);
                    enemy.SetActive(false);

                    EnemyMovement enemyMovement = enemyType == EnemyType.Earth
                        ? enemy.AddComponent<EarthEnemyMovement>()
                        : enemy.AddComponent<WaterEnemyMovement>();

                    enemyMovement.Initialize(playerMovement, tileManager, cameraService, playerHealth);
                    Add(enemy, enemyType);
                }
            }
        }

        public void Add(GameObject enemy, EnemyType enemyType)
        {
            switch (enemyType)
            {
                default:
                case EnemyType.Earth:
                    enemiesEarth.Add(enemy);
                    break;
                case EnemyType.Water:
                    enemiesWater.Add(enemy);
                    break;
            }
        }

        public GameObject Get(EnemyType enemyType)
        {
            GameObject enemy;
            switch (enemyType)
            {
                default:
                case EnemyType.Earth:
                    enemy = enemiesEarth[0];
                    enemiesEarth.Remove(enemiesEarth[0]);
                    break;
                case EnemyType.Water:
                    enemy = enemiesWater[0];
                    enemiesWater.Remove(enemiesWater[0]);
                    break;
            }

            return enemy;
        }
    }
}