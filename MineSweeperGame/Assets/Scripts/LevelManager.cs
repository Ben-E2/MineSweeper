using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    

    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tilemap UnderTilemap;

    [SerializeField] private TileBase BaseTile;

    public BoundsInt LevelBounds;

    [Range(0f, 1f)] public float MineAmount;

    [SerializeField] private float MineSpawnChance = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        LevelBounds.size = new Vector3Int(10, 10, 1);
        SetLevelBoundsPosition();
    }

    private void SetLevelBoundsPosition()
    {
        int _levelBoundsMin = (-LevelBounds.size.x / 2);

        LevelBounds.position = new Vector3Int(_levelBoundsMin, _levelBoundsMin, 0);
    }

    public void OnTileClick(Vector3Int clickPosition)
    {
        if (MainTilemap.ContainsTile(BaseTile)) 
        {
            MainTilemap.SetTile(clickPosition, null);
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 _center = LevelBounds.position + (Vector3)LevelBounds.size / 2;

        Vector3 _size = (Vector3)LevelBounds.size;
        Gizmos.DrawWireCube(_center, _size);
    }
}
