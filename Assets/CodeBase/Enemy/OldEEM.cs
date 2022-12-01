// namespace CodeBase.Enemy
// {
//     public class OldEEM
//     {
//         using System.Collections;
// using System.Linq;
// using CodeBase.Tile;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace CodeBase.Enemy
// {
//     public class EarthEnemyMovement : EnemyMovement
//     {
//         private readonly float degree = 45f;
//         private Vector3 position;
//         private Vector3 positionBeforeLap;
//         private Vector2Int direction;
//         private TileState tileState;
//
//         public override void StartMovement()
//         {
//             tileState = TileState.Painted;
//             int randomNumber = Random.Range(0, 5);
//             direction = randomNumber switch
//             {
//                 1 => new Vector2Int(-1, 1),
//                 2 => new Vector2Int(1, -1),
//                 3 => new Vector2Int(1, 1),
//                 _ => new Vector2Int(-1, -1)
//             };
//             // direction = new Vector2Int(1, 0);
//             StartCoroutine(EndlessMovement());
//         }
//
//         public override void StopMovement()
//         {
//             StopCoroutine(EndlessMovement());
//             playerMovement.WasHit = false;
//         }
//
//         private IEnumerator EndlessMovement()
//         {
//             while (true)
//             {
//                 position = transform.position;
//
//                 Vector2Int positionInt = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
//                 //GetComponent<SpriteRenderer>().bounds.Intersects();
//                 // Bounds? tempBounds = null;
//                 //
//                 // if (bounds.bounds.Intersects(tileManager.Tiles[positionInt].Bounds)
//                 //     && tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                 //     tempBounds = tileManager.Tiles[positionInt].Bounds;
//                 //
//                 // else if (bounds.bounds.Intersects(tileManager.Tiles[positionInt + Vector2Int.right].Bounds)
//                 //          && tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                 //     tempBounds = tileManager.Tiles[positionInt + Vector2Int.right].Bounds;
//                 //
//                 // else if (bounds.bounds.Intersects(tileManager.Tiles[positionInt + Vector2Int.down].Bounds)
//                 //          && tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                 //     tempBounds = tileManager.Tiles[positionInt + Vector2Int.down].Bounds;
//                 //
//                 // else if (bounds.bounds.Intersects(tileManager.Tiles[positionInt + Vector2Int.down + Vector2Int.right].Bounds)
//                 //          && tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                 //     tempBounds = tileManager.Tiles[positionInt + Vector2Int.right + Vector2Int.down].Bounds;
//                 //
//                 // if (tempBounds != null)
//                 // {
//                 //     Reflect((Bounds) tempBounds);
//                 // }
//                 // if (tileManager.Tiles.ContainsKey(positionInt))
//                 // {
//                 //     if (tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                 //     {
//                 //         enemyDestroyCallback?.Invoke();
//                 //     }
//                 // }
//
//
//                 Vector3 moveDestination = new Vector3(position.x + direction.x, position.y + direction.y, 0);
//
//                 Vector2Int intVector = new(Mathf.FloorToInt(moveDestination.x), Mathf.FloorToInt(moveDestination.y));
//
//
//                 if (tileManager.Tiles.ContainsKey(intVector))
//                 {
//                     if (tileManager.Tiles[intVector].TileState == tileState)
//                     {
//                         int randomNumber = Random.Range(0, 5);
//                         Vector2Int tempDirection = randomNumber switch
//                         {
//                             1 => new Vector2Int(-1, 1),
//                             2 => new Vector2Int(1, -1),
//                             3 => new Vector2Int(1, 1),
//                             _ => new Vector2Int(-1, -1)
//                         };
//                         if (tempDirection == direction)
//                             direction *= -1;
//                         else
//                             direction = tempDirection;
//
//                         // var x = Random.Range(-1, 2);
//                         // var y = Random.Range(-1, 2);
//                         // if (x == 0 && y == 0)
//                         //     direction = new Vector2Int(-1, 1);
//                         // else
//                         //     direction = new Vector2Int(x, y);
//                     }
//                 }
//
//                 if (tileManager.Tiles.ContainsKey(positionInt))
//                 {
//                     if (tileManager.Tiles[positionInt].TileState == TileState.Painted)
//                         enemyDestroyCallback?.Invoke();
//                 }
//
//                 if (playerMovement.EnemyInCheck(positionInt) ||
//                     playerMovement.TilesInProcess.Any(tile => tile.Position == intVector))
//                 {
//                     playerHealth.DecreaseHealth();
//                     position = new Vector3(3, 3, transform.position.z);
//                     moveDestination = position;
//                 }
//
//                 // if (playerMovement.EnemyInCheck(intVector))
//                 // {
//                 //     playerHealth.DecreaseHealth();
//                 //     position = new Vector3(3, 3, transform.position.z);
//                 //     moveDestination = position;
//                 // }
//
//                 // else if (playerMovement.TilesInProcess.Any(tile => tile.Position == intVector))
//                 // {
//                 //     playerHealth.DecreaseHealth();
//                 //     playerMovement.wasHit = true;
//                 // }
//
//                 transform.position = Vector3.Lerp(position, moveDestination, 5 * Time.deltaTime);
//                 positionBeforeLap = transform.position;
//                 yield return null;
//             }
//         }
//
//         private void Reflect(Bounds to)
//         {
//             Vector2 reflectedPoint = transform.position;
//             // var angle = Mathf.Abs(Vector2.Angle(position - to.center, Vector2.right));
//             var angle = Mathf.Atan2(transform.position.y - to.center.y, transform.position.x - to.center.x);
//
//             if (degree > angle && angle < Mathf.PI - degree || angle > -Mathf.PI + degree && angle < -degree)
//             {
//                 direction = new Vector2Int(direction.x, -direction.y);
//                 //transform.position.Set(transform.position.x, -transform.position.y, transform.position.z);
//                 //reflectedPoint.y = 2 * transform.position.y - positionBeforeLap.y;
//                 reflectedPoint.y = (angle > 0.0f ? to.extents.y + to.size.y : to.center.y - transform.position.y);
//             }
//             else
//             {
//                 direction = new Vector2Int(-direction.x, direction.y);
//                 //transform.position.Set(-transform.position.x, transform.position.y, transform.position.z);
//                 //reflectedPoint.x = 2 * transform.position.x - positionBeforeLap.x;
//                 reflectedPoint.x = (angle <= degree && angle >= degree ? to.center.x - transform.position.x : to.center.x + to.size.x);
//             }
//
//             transform.position = reflectedPoint;
//         }
//     }
// }
//     }
// }