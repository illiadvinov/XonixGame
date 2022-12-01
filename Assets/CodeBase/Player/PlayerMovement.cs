using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Camera;
using CodeBase.Tile;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public List<TileInfo> TilesInProcess { get; private set; }
        public bool WasHit;
        [SerializeField] private float movementSpeed = 5;
        private InputSystem.InputSystem inputSystem;
        private Vector3 direction;
        private Vector3 position;
        private TileManager tileManager;
        private float width, height;
        private Vector2Int intPosition;

        [Inject]
        public void Construct(CameraService cameraService, TileManager tileManager, InputSystem.InputSystem inputSystem)
        {
            width = cameraService.Width;
            height = cameraService.Height;
            this.tileManager = tileManager;
            this.inputSystem = inputSystem;
            TilesInProcess = new List<TileInfo>();
        }

        private void Awake()
        {
            inputSystem.OnDirectionChanged += AlignPlayer;
        }

        private void OnEnable()
        {
            transform.position = new Vector3(0, Mathf.FloorToInt(height) - 2, 0);
            direction = Vector3.right;
            WasHit = false;

        }

        private void Update()
        {
            position = transform.position;

            Vector3 moveDestination = new Vector3(position.x + inputSystem.Direction.x,
                position.y + inputSystem.Direction.y, -1);


            if (inputSystem.Direction == Vector2Int.left || inputSystem.Direction == Vector2Int.down)
                intPosition = new Vector2Int(Mathf.CeilToInt(position.x), Mathf.CeilToInt(position.y));
            else
                intPosition = new Vector2Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));

            MovePlayer(moveDestination);
            if (!WasHit)
            {
                SetTileToProcess(intPosition);
                PaintTile(intPosition);
            }
        }

        private void OnDisable()
        {
            WasHit = false;
            TilesInProcess.Clear();

        }

        public bool EnemyInCheck(Vector2Int enemyPosition)
        {
            if (enemyPosition == intPosition)
                return true;
            return false;
        }


        private void MovePlayer(Vector3 moveDestination)
        {
            if (moveDestination.x > width || moveDestination.x < 0)
            {
                moveDestination.x = Mathf.Abs(moveDestination.x - width);
                transform.position = moveDestination;
            }
            else if (moveDestination.y > height || moveDestination.y < 0)
            {
                moveDestination.y = Mathf.Abs(moveDestination.y - height);
                transform.position = moveDestination;
            }
            else
            {
                transform.position = Vector3.Lerp(position, moveDestination, movementSpeed * Time.deltaTime);
            }
        }

        private void AlignPlayer()
        {
            transform.position = new Vector2(Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.y));
        }

        private void PaintTile(Vector2Int intVector)
        {
            if (tileManager.Tiles.ContainsKey(intVector)
                && tileManager.Tiles[intVector].TileState == TileState.Painted)
            {
                tileManager.PaintTilesInProcess(TilesInProcess, tileManager.PaintedPercentage);

                TilesInProcess.Clear();
            }
        }

        private void SetTileToProcess(Vector2Int intVector)
        {
            if (tileManager.Tiles.ContainsKey(intVector) && tileManager.Tiles[intVector].TileState == TileState.Empty)
            {
                tileManager.Tiles[intVector].InProcess();
                TilesInProcess.Add(tileManager.Tiles[intVector]);
            }
        }
    }
}