using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Tilemap MainTileMap;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) { Debug.Log(GetHoveredTile()); }
    }

    private Vector3Int GetHoveredTile()
    {
        var _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return MainTileMap.WorldToCell(_mousePosition);
    }
}
