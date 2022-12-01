using System;
using CodeBase.Camera;
using CodeBase.Player;
using CodeBase.Tile;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public Action enemyDestroyCallback;
    protected PlayerMovement playerMovement;
    protected TileManager tileManager;
    protected PlayerHealth playerHealth;
    protected float cameraWidth, cameraHeight;
    protected SpriteRenderer bounds;

    public void Initialize(
        PlayerMovement playerMovement,
        TileManager tileManager,
        CameraService cameraService,
        PlayerHealth playerHealth)
    {
        this.playerMovement = playerMovement;
        this.tileManager = tileManager;
        cameraWidth = cameraService.Width;
        cameraHeight = cameraService.Height;
        this.playerHealth = playerHealth;
        bounds = GetComponent<SpriteRenderer>();
        //extents = (Vector2Int) bounds.extents;
    }

    public abstract void StartMovement();
    public abstract void StopMovement();
}