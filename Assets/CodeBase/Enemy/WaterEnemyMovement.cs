using System;
using System.Collections;
using CodeBase.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Enemy
{
    public class WaterEnemyMovement : EnemyMovement
    {
        private Vector3 position;
        private Vector2 direction;
        private TileState tileState;
        private bool isXMovement;

        public override void StartMovement()
        {
            tileState = TileState.Empty;
            int randomNumber = Random.Range(0, 2);
            direction = randomNumber == 1 ? new Vector2Int(1, 0) : new Vector2Int(0, 1);
            StartCoroutine(EndlessMovement());
        }

        public override void StopMovement() =>
            StopCoroutine(EndlessMovement());

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
                            1 => new Vector2Int(-1, 0),
                            2 => new Vector2Int(1, 0),
                            3 => new Vector2Int(0, 1),
                            _ => new Vector2Int(0, -1)
                        };

                        if (direction == tempDirection)
                            direction *= -1;
                        else
                            direction = tempDirection;
                    }
                }

                if (playerMovement.EnemyInCheck(positionInt))
                    playerHealth.DecreaseHealth();

                transform.position = Vector3.Lerp(position, moveDestination, 5 * Time.deltaTime);
                yield return null;
            }
        }
    }
}