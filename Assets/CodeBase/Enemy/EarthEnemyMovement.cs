using System.Collections;
using System.Linq;
using CodeBase.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class EarthEnemyMovement : EnemyMovement
    {
        private readonly float degree = 45f;
        private Vector3 position;
        private Vector3 positionBeforeLap;
        private Vector2Int direction;
        private TileState tileState;

        public override void StartMovement()
        {
            tileState = TileState.Painted;
            int randomNumber = Random.Range(0, 5);
            direction = randomNumber switch
            {
                1 => new Vector2Int(-1, 1),
                2 => new Vector2Int(1, -1),
                3 => new Vector2Int(1, 1),
                _ => new Vector2Int(-1, -1)
            };
            StartCoroutine(EndlessMovement());
        }

        public override void StopMovement()
        {
            StopCoroutine(EndlessMovement());
            playerMovement.WasHit = false;
        }

        private IEnumerator EndlessMovement()
        {
            while (true)
            {
                position = transform.position;

                Vector2Int positionInt = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));


                Vector3 moveDestination = new Vector3(position.x + direction.x, position.y + direction.y, 0);

                Vector2Int intVector = new(Mathf.FloorToInt(moveDestination.x), Mathf.FloorToInt(moveDestination.y));


                if (tileManager.Tiles.ContainsKey(intVector))
                {
                    if (tileManager.Tiles[intVector].TileState == tileState)
                    {
                        int randomNumber = Random.Range(0, 5);
                        Vector2Int tempDirection = randomNumber switch
                        {
                            1 => new Vector2Int(-1, 1),
                            2 => new Vector2Int(1, -1),
                            3 => new Vector2Int(1, 1),
                            _ => new Vector2Int(-1, -1)
                        };
                        if (tempDirection == direction)
                            direction *= -1;
                        else
                            direction = tempDirection;
                    }
                }

                if (tileManager.Tiles.ContainsKey(positionInt))
                {
                    if (tileManager.Tiles[positionInt].TileState == TileState.Painted)
                        enemyDestroyCallback?.Invoke();
                }

                if (playerMovement.EnemyInCheck(positionInt) ||
                    playerMovement.TilesInProcess.Any(tile => tile.Position == intVector))
                {
                    playerHealth.DecreaseHealth();
                    position = new Vector3(3, 3, transform.position.z);
                    moveDestination = position;
                }

                transform.position = Vector3.Lerp(position, moveDestination, 5 * Time.deltaTime);
                positionBeforeLap = transform.position;
                yield return null;
            }
        }
    }
}