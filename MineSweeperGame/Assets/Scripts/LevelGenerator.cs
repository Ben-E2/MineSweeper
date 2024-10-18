using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private GameManager GameManager;

    [Space]
    [SerializeField] private Tilemap FlagTilemap;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tilemap UnderTilemap;

    [Space]
    [SerializeField] private TileBase CoverTileBase;
    [SerializeField] private TileBase MineTileBase;

    [Space]
    [SerializeField] private TileBase Num1TileBase;
    [SerializeField] private TileBase Num2TileBase;
    [SerializeField] private TileBase Num3TileBase;
    [SerializeField] private TileBase Num4TileBase;
    [SerializeField] private TileBase Num5TileBase;
    [SerializeField] private TileBase Num6TileBase;
    [SerializeField] private TileBase Num7TileBase;
    [SerializeField] private TileBase Num8TileBase;

    public IEnumerator CreateLevel()
    {
        yield return StartCoroutine("ClearTilemaps");

        yield return StartCoroutine("CreateGridData");

        yield return StartCoroutine("PlaceMines");

        yield return StartCoroutine("DrawMainLayer");

        yield return StartCoroutine("DrawUnderLayer");
    }

    // Initialises the GridData Dictionary with all of the cells.
    private IEnumerator CreateGridData()
    {
        LevelManager.GridData = new Dictionary<Vector3Int, TileData>();

        for (int x = 0; x < LevelManager.LevelBounds.size.x; x++)
        {
            for (int y = 0; y < LevelManager.LevelBounds.size.y; y++)
            {
                LevelManager.GridData[new Vector3Int(x, y, 0)] = new TileData(false, 0, false, false);
            }
        }

        yield return null;
    }

    private IEnumerator PlaceMines()
    {
        int _levelArea = LevelManager.LevelBounds.size.x * LevelManager.LevelBounds.size.y;
        int _maxMines = (int)(_levelArea * LevelManager.MaxMinePercentage);

        int _placedMines = 0;
        while (_placedMines < _maxMines)
        {
            for (int x = 0; x < LevelManager.LevelBounds.size.x; x++)
            {
                for (int y = 0; y < LevelManager.LevelBounds.size.y; y++)
                {
                    if (_placedMines >= _maxMines) { continue; }

                    float _placeMineRNG = Random.Range(0f, 1f);
                    if (_placeMineRNG <= LevelManager.MineSpawnChance)
                    {
                        LevelManager.GridData[new Vector3Int(x, y, 0)] = new TileData(true, 0, false, false);

                        _placedMines++;
                    }
                }
            }
        }

        GameManager.RemainingFlags = _placedMines;
        GameManager.AmountOfMines = _placedMines;

        yield return null;
        
    }

    private void DrawMines()
    {
        foreach (KeyValuePair<Vector3Int, TileData> tile in LevelManager.GridData)
        {
            Vector3Int _key = tile.Key;
            TileData _data = tile.Value;

            if (_data.isMine)
            {
                UnderTilemap.SetTile(_key, MineTileBase);
            }
        }
    }

    private void DrawUnderLayer()
    {
        DrawMines();

        foreach (KeyValuePair<Vector3Int, TileData> tile in LevelManager.GridData)
        {
            Vector3Int _key = tile.Key;
            TileData _data = tile.Value;

            // If the tile is a mine, skip.
            if (_data.isMine)
            {
                continue;
            }

            int _surroundingMines = CalculateSurroundingMines(_key);
            switch (_surroundingMines)
            {
                case 1:
                    UnderTilemap.SetTile(_key, Num1TileBase);
                    break;

                case 2:
                    UnderTilemap.SetTile(_key, Num2TileBase);
                    break;

                case 3:
                    UnderTilemap.SetTile(_key, Num3TileBase);
                    break;

                case 4:
                    UnderTilemap.SetTile(_key, Num4TileBase);
                    break;

                case 5:
                    UnderTilemap.SetTile(_key, Num5TileBase);
                    break;

                case 6:
                    UnderTilemap.SetTile(_key, Num6TileBase);
                    break;

                case 7:
                    UnderTilemap.SetTile(_key, Num7TileBase);
                    break;

                case 8:
                    UnderTilemap.SetTile(_key, Num8TileBase);
                    break;
            }
        }
    }

    private void DrawMainLayer()
    {
        foreach (KeyValuePair<Vector3Int, TileData> tile in LevelManager.GridData)
        {
            Vector3Int _key = tile.Key;

            MainTilemap.SetTile(_key, CoverTileBase);
        }
    }

    private IEnumerator ClearTilemaps()
    {
        FlagTilemap.ClearAllTiles();

        MainTilemap.ClearAllTiles();

        UnderTilemap.ClearAllTiles();

        yield return null;
    }

    private int CalculateSurroundingMines(Vector3Int tilePosition)
    {
        List<Vector3Int> _directions = new List<Vector3Int>() 
        { 
            new Vector3Int (-1, 1, 0), // Top left
            new Vector3Int (0, 1, 0), // Top middle
            new Vector3Int (1, 1, 0), // Top right
            new Vector3Int (1, 0, 0), // Middle right
            new Vector3Int (1, -1, 0), // Bottom right
            new Vector3Int (0, -1, 0), // Bottom middle
            new Vector3Int (-1, -1, 0), // Bottom left
            new Vector3Int (-1, 0, 0), // Middle left
        };

        int _surroundingMines = 0;
        foreach (Vector3Int direction in _directions)
        {
            Vector3Int _neighbourPosition = tilePosition + direction;

            if (LevelManager.GridData.ContainsKey(_neighbourPosition) == false) { continue; }

            if (LevelManager.GridData[_neighbourPosition].isMine)
            {
                _surroundingMines++;
            }
        }

        LevelManager.GridData[tilePosition].surroundingMines = _surroundingMines;

        return _surroundingMines;
    }
}
