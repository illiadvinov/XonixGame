using System;
using UnityEngine;

namespace CodeBase.Tile
{
    public class TileInfo : MonoBehaviour
    {
        public Vector2Int Position;
        public Vector3 Extents { get; private set; }
        public Bounds Bounds { get; private set; }
        public TileState TileState { get; private set; }
        public bool CanBePainted = true;
        [SerializeField] private SpriteRenderer spriteRenderer;


        private void Awake()
        {
            Bounds = GetComponent<SpriteRenderer>().bounds;
            Extents = Bounds.extents;
        }

        public void InProcess()
        {
            if (!CanBePainted)
                return;

            this.TileState = TileState.Process;
            spriteRenderer.color = Color.cyan;
        }

        public bool Paint()
        {
            if (!CanBePainted || TileState == TileState.Painted)
                return false;

            this.TileState = TileState.Painted;
            spriteRenderer.color = Color.green;
            return true;
        }

        public void Empty()
        {
            this.TileState = TileState.Empty;
            spriteRenderer.color = Color.gray;
        }
    }

    public enum TileState
    {
        Empty = 0,
        Process = 1,
        Painted = 2
    }
}