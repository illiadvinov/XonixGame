using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyManager
    {
        private readonly EnemyPool enemyPool;
        private readonly Transform activeEnemiesContainer;
        private readonly Transform enemyParent;
        private List<(EnemyType type, GameObject enemy)> enemiesOnScene;

        [Inject]
        public EnemyManager(
            EnemyPool enemyPool,
            [Inject(Id = "ActiveEnemiesContainer")]
            Transform activeEnemiesContainer,
            [Inject(Id = "EnemyParent")] Transform enemyParent)
        {
            this.enemyPool = enemyPool;
            this.activeEnemiesContainer = activeEnemiesContainer;
            this.enemyParent = enemyParent;
            enemiesOnScene = new List<(EnemyType, GameObject)>();
        }

        public void SpawnEnemies(int level)
        {
            for (int i = 0; i < level; i++)
            {
                EnemyType type = (EnemyType) (i % 3);
                Vector2 startPosition = Vector2.zero;
                if (type == EnemyType.Earth)
                    startPosition = new Vector2(3, 3);

                var enemy = GetEnemy(type, startPosition);
                enemy.GetComponent<EnemyMovement>().enemyDestroyCallback = () => DisableEnemy(enemy, type);
                enemiesOnScene.Add((type, enemy));
            }
        }

        public void StartEnemiesMovement()
        {
            foreach ((EnemyType type, GameObject enemy) tuple in enemiesOnScene)
                tuple.enemy.GetComponent<EnemyMovement>().StartMovement();
        }

        public void StopEnemies()
        {
            foreach ((EnemyType type, GameObject enemy) tuple in enemiesOnScene)
                StopEnemy(tuple.enemy);
        }

        public void DisableEnemies()
        {
            foreach ((EnemyType type, GameObject enemy) tuple in enemiesOnScene) 
                DisableEnemy(tuple.enemy, tuple.type);

            enemiesOnScene.Clear();
        }

        private void StopEnemy(GameObject enemy) =>
            enemy.GetComponent<EnemyMovement>().StopMovement();

        private void DisableEnemy(GameObject enemy, EnemyType type)
        {
            StopEnemy(enemy);
            enemy.SetActive(false);
            enemy.transform.SetParent(enemyParent);
            enemyPool.Add(enemy, type);
        }

        private GameObject GetEnemy(EnemyType enemyType, Vector2 spawnPosition)
        {
            var enemy = enemyPool.Get(enemyType);
            enemy.transform.SetParent(activeEnemiesContainer);
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);
            return enemy;
        }
    }
}