using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool isMine;
    public int surroundingMines;
    public bool isRevealed;
    public bool isFlagged;

    public TileData(bool isMine, int surroundingMines, bool isRevealed, bool isFlagged)
    {
        this.isMine = isMine;
        this.surroundingMines = surroundingMines;
        this.isRevealed = isRevealed;
        this.isFlagged = isFlagged;
    }
}

public class LevelManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelGenerator LevelGenerator;
    [SerializeField] private GameManager GameManager;

    [Space]
    [SerializeField] private Tilemap FlagTilemap;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tilemap UnderTilemap;

    [Space]
    [SerializeField] private TileBase FlagTile;
    [SerializeField] private TileBase CoverTile;

    [Space]
    public BoundsInt LevelBounds;
    public Dictionary<Vector3Int, TileData> GridData  = new Dictionary<Vector3Int, TileData>();

    [Space]
    [Range(0.1f, 0.9f)] public float MaxMinePercentage;
    [Range(0.01f, 0.1f)] public float MineSpawnChance;

    #endregion

    public void CreateNewLevel()
    {
        // Will also clear score and other data when implemented.

        StartCoroutine(LevelGenerator.CreateLevel());
    }

    public void OnTileM1Click(Vector3Int clickPosition)
    {
        if (GridData[clickPosition].isFlagged)
        {
            Debug.LogWarning("Can't click, tile is flagged.");

            return;
        }
        else if(GridData[clickPosition].isMine)
        {
            Debug.LogWarning("Game over.");

            GameManager.CurrentGameState = GameState.GameLost;
        }

        if (GridData[clickPosition].isRevealed == false)
        {
            GridData[clickPosition].isRevealed = true;

            GameManager.RemainingTiles -= 1;

            MainTilemap.SetTile(clickPosition, null);

            GameManager.CheckIfGameWon();
        } 
    }

    public void OnTileM2Click(Vector3Int clickPosition)
    {
        if (GridData[clickPosition].isFlagged)
        {
            GameManager.ChangeFlagAmount(1);

            GridData[clickPosition].isFlagged = false;

            FlagTilemap.SetTile(clickPosition, null);
        }
        else if (!GridData[clickPosition].isFlagged && GameManager.RemainingFlags > 0)
        {
            GameManager.ChangeFlagAmount(-1);

            GridData[clickPosition].isFlagged = true;

            FlagTilemap.SetTile(clickPosition, FlagTile);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 _center = LevelBounds.center;

        Vector3 _size = (Vector3)LevelBounds.size;
        Gizmos.DrawWireCube(_center, _size);
    }
}
