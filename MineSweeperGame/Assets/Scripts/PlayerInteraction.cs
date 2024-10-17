using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private Tilemap MainTileMap;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            Vector3Int _mapPosition = GetHoveredTile();

            if (CheckTileIsValid(_mapPosition)) 
            {
                Debug.Log(_mapPosition);
                LevelManager.OnTileClick(_mapPosition);
            }
        }
    }

    private Vector3Int GetHoveredTile()
    {
        var _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int _mapPosition = MainTileMap.WorldToCell(_mousePosition);

        return _mapPosition;
    }

    private bool CheckTileIsValid(Vector3Int mapPosition)
    {
        return LevelManager.LevelBounds.Contains(mapPosition);
    }
}
