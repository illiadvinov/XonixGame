using System;
using System.Collections.Generic;
using CodeBase.UI;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Tile
{
    public class TileManager
    {
        public Dictionary<Vector2, TileInfo> Tiles { get; private set; }
        private PlayerProgress playerProgress;
        private int paintedCount = 0;
        private int amountOfEmptyTiles = 0;

        [Inject]
        public void Construct(PlayerProgress playerProgress)
        {
            this.playerProgress = playerProgress;
            Tiles = new Dictionary<Vector2, TileInfo>();
        }

        public void InitializeDictionary(Vector2 position, TileInfo tileInfo)
        {
            Tiles.Add(position, tileInfo);
        }

        public void ResetPaintedTilesCount() =>
            paintedCount = 0;

        public void CountEmptyTilesAmount(int tilesNotToCount) =>
            amountOfEmptyTiles = Tiles.Count - tilesNotToCount;

        public void PaintTilesInProcess(List<TileInfo> tilesInProcess, Action callback)
        {
            if (tilesInProcess.Count <= 0)
                return;

            int xMax = tilesInProcess[0].Position.x, xMin = tilesInProcess[0].Position.x;
            int yMax = tilesInProcess[0].Position.y, yMin = tilesInProcess[0].Position.y;

            Vector2Int minTile = tilesInProcess[0].Position, maxTile = tilesInProcess[0].Position;

            foreach (TileInfo tileInfo in tilesInProcess)
            {
                int positionX = tileInfo.Position.x;
                int positionY = tileInfo.Position.y;

                if (xMax < positionX)
                    xMax = positionX;
                if (xMin > positionX)
                    xMin = positionX;
                if (yMax < positionY)
                    yMax = positionY;
                if (yMin > positionY)
                    yMin = positionY;

                if (maxTile.x + maxTile.y < tileInfo.Position.x + tileInfo.Position.y)
                    maxTile = tileInfo.Position;

                if (minTile.x + minTile.y > tileInfo.Position.x + tileInfo.Position.y)
                    minTile = tileInfo.Position;
            }


            for (int y = yMin; y <= yMax; y++)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    if (Tiles[new Vector2(x, y)].Paint())
                        paintedCount++;
                }
            }

            callback?.Invoke();
        }

        public void PaintedPercentage()
        {
            float t = (float) paintedCount / (float) amountOfEmptyTiles;
            int percentage = (int) (t * 100);
            playerProgress.SetPaintProgress(percentage);
        }
    }
}