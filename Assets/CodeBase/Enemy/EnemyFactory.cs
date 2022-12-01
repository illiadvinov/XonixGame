using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyFactory
    {
        private const string earthEnemyPath = "EnemyEarth";
        private const string waterEnemyPath = "EnemyWater";

        public GameObject LoadEnemy(EnemyType enemyType, Transform parent = null)
        {
            GameObject gameObject;
            switch (enemyType)
            {
                default:
                case EnemyType.Earth:
                    gameObject = Resources.Load<GameObject>(earthEnemyPath);
                    break;
                case EnemyType.Water:
                    gameObject = Resources.Load<GameObject>(waterEnemyPath);
                    break;
            }

            return Object.Instantiate(gameObject, parent) as GameObject;
        }
    }
}