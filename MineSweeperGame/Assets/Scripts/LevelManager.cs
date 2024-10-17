using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileData
{
    public bool isMine;
    public bool isRevealed;
    public bool isFlagged;

    public TileData(bool isMine, bool isRevealed, bool isFlagged)
    {
        this.isMine = isMine;
        this.isRevealed = isRevealed;
        this.isFlagged = isFlagged;
    }
}

public class LevelManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelGenerator LevelGenerator;

    [Space]
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tilemap UnderTilemap;

    [Space]
    [SerializeField] private TileBase CoverTile;

    [Space]
    public BoundsInt LevelBounds;
    public Dictionary<Vector3Int, TileData> GridData  = new Dictionary<Vector3Int, TileData>();

    [Space]
    [Range(0, 500)] public float MineAmount;
    [Range(0.1f, 0.9f)] [SerializeField] private float MaxMinePercent;
    [Range(0.01f, 0.1f)] public float MineSpawnChance = 0.05f;

    #endregion

    void Start()
    {
        LevelBounds.size = new Vector3Int(10, 10, 1);

        ValidateMineAmount();
    }

    public void OnTileClick(Vector3Int clickPosition)
    {
        if (MainTilemap.ContainsTile(CoverTile)) 
        {
            MainTilemap.SetTile(clickPosition, null);
        } 
    }

    public void ValidateMineAmount()
    {
        int _playArea = LevelBounds.size.x * LevelBounds.size.y;

        if (MineAmount > _playArea * MaxMinePercent)
        {
            MineAmount = _playArea * MaxMinePercent;
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
