using System.Collections.Generic;
using CodeBase.Camera;
using CodeBase.Tile;
using UnityEngine;
using Zenject;

namespace CodeBase.Grid
{
    public class GridManager
    {
        private const string tilePrefabPath = "Tile";
        private Transform tileParent;
        private CameraService cameraService;
        private TileManager tileManager;
        private List<GameObject> emptyTiles;

        [Inject]
        public void Construct(
            CameraService cameraService,
            TileManager tileManager,
            [Inject(Id = "TileParent")] Transform tileParent)
        {
            this.cameraService = cameraService;
            this.tileManager = tileManager;
            emptyTiles = new List<GameObject>();
            this.tileParent = tileParent;
        }

        public void Reset()
        {
            foreach (GameObject tile in emptyTiles)
                tile.GetComponent<TileInfo>().Empty();
        }

        public void Initialize()
        {
            int tilesNotToCount = 0;

            GameObject tileGameObject = Resources.Load<GameObject>(tilePrefabPath);

            for (int i = 0; i < Mathf.CeilToInt(cameraService.Height); i++)
            {
                for (int j = 0; j < Mathf.CeilToInt(cameraService.Width); j++)
                {
                    GameObject tile = Object.Instantiate(tileGameObject,
                        new Vector3(j, i, tileParent.transform.position.z),
                        Quaternion.identity,
                        tileParent);
                    TileInfo tileInfo = tile.GetComponent<TileInfo>();

                    InitializeTileParams(tileInfo, j, i, tile, true);

                    if (i == 0 || j == 0 || i == (int) cameraService.Height - 1 || j == (int) cameraService.Width)
                    {
                        tileInfo.Paint();
                        tilesNotToCount++;
                        continue;
                    }

                    emptyTiles.Add(tile);
                }
            }

            cameraService.SetCameraPosition();

            CreateEdgeTiles(tileGameObject, ref tilesNotToCount);
            tileManager.CountEmptyTilesAmount(tilesNotToCount);
        }

        private void CreateEdgeTiles(GameObject tileGameObject, ref int tilesNotToCount)
        {
            for (int x = -1; x <= Mathf.CeilToInt(cameraService.Width) + 1; x += Mathf.CeilToInt(cameraService.Width) + 1)
            {
                for (int y = -1; y < Mathf.CeilToInt(cameraService.Height) + 1; y++)
                    CreateEdgeTile(x, y, tileGameObject, ref tilesNotToCount);
            }

            for (int y = -1; y <= Mathf.CeilToInt(cameraService.Height) + 1; y += Mathf.CeilToInt(cameraService.Height) + 1)
            {
                for (int x = -1; x < Mathf.CeilToInt(cameraService.Width) + 1; x++)
                    CreateEdgeTile(x, y, tileGameObject, ref tilesNotToCount);
            }
        }

        private void CreateEdgeTile(int x, int y, GameObject tileGameObject, ref int tilesNotToCount)
        {
            GameObject edgeTile = Object.Instantiate(tileGameObject,
                new Vector3(x, y, tileParent.transform.position.z),
                Quaternion.identity,
                tileParent);
            if (!tileManager.Tiles.ContainsKey(new Vector2(x, y)))
            {
                InitializeTileParams(edgeTile.GetComponent<TileInfo>(), x, y, edgeTile, false);
                tilesNotToCount++;
            }
        }

        private void InitializeTileParams(TileInfo tileInfo, int x, int y, GameObject tile, bool canBePainted)
        {
            tileInfo.Position = new Vector2Int(x, y);
            tile.name = $"Tile:{x},{y}";
            tileManager.InitializeDictionary(new Vector2(x, y), tileInfo);
            tileInfo.Empty();
            tileInfo.CanBePainted = canBePainted;
        }
    }
}