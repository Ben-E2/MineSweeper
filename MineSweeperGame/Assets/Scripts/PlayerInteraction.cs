using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LevelManager LevelManager;

    [Space]
    [SerializeField] private Tilemap MainTileMap;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) 
        {
            Vector3Int _mapPosition = GetHoveredTile();

            if (CheckTileIsValid(_mapPosition)) 
            {
                LevelManager.OnTileM1Click(_mapPosition);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            Vector3Int _mapPosition = GetHoveredTile();

            if (CheckTileIsValid(_mapPosition))
            {
                LevelManager.OnTileM2Click(_mapPosition);
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
        return LevelManager.GridData.ContainsKey(mapPosition);
    }
}
