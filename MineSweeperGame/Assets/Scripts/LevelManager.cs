using System;
using System.Collections;
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

    #region Tilemaps
    [Space]
    [SerializeField] private Tilemap FlagTilemap;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tilemap UnderTilemap;
    #endregion

    #region Tile Assets
    [Space]
    [SerializeField] private TileBase FlagTile;
    [SerializeField] private TileBase MineTile;
    [SerializeField] private TileBase CoverTile;
    #endregion

    #region Level Information
    [Space]
    public BoundsInt LevelBounds;
    [HideInInspector] public int LevelArea;
    public Dictionary<Vector3Int, TileData> GridData  = new Dictionary<Vector3Int, TileData>();

    [Space]
    [Range(0.1f, 0.9f)] public float MaxMinePercentage;
    [Range(0.01f, 0.1f)] public float MineSpawnChance;
    #endregion

    #endregion

    public void CreateNewLevel()
    {
        // Will also clear score and other data when implemented.

        GameManager.OnGoingGame = true;

        StartCoroutine(LevelGenerator.CreateLevel());

        LevelArea = LevelBounds.size.x * LevelBounds.size.y;

        GameManager.TimeTaken = 0f;
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
            GameManager.CurrentGameState = GameState.GameLost;

            return;
        }

        if (GridData[clickPosition].isRevealed == false)
        {
            if (GridData[clickPosition].surroundingMines <= 0)
            {
                FloodFill(clickPosition);
            }
            else
            {
                RevealTile(clickPosition);
            }

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
        else if (!GridData[clickPosition].isFlagged && !GridData[clickPosition].isRevealed && GameManager.RemainingFlags > 0)
        {
            GameManager.ChangeFlagAmount(-1);

            GridData[clickPosition].isFlagged = true;

            FlagTilemap.SetTile(clickPosition, FlagTile);
        }
    }

    private void FloodFill(Vector3Int clickPosition)
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

        List<Vector3Int> _tempFloodList = new List<Vector3Int>();
        List<Vector3Int> _tempIngoreList = new List<Vector3Int>();

        _tempFloodList.Add(clickPosition);

        int _index = 0;
        while (_index < _tempFloodList.Count) 
        {
            Vector3Int _tile = _tempFloodList[_index];

            foreach (Vector3Int direction in _directions)
            {
                Vector3Int _currentTile = _tile + direction;

                if (!GridData.ContainsKey(_currentTile) || GridData[_currentTile].isRevealed)
                {
                    continue;
                }

                if (GridData[_currentTile].surroundingMines == 0 && !_tempFloodList.Contains(_currentTile))
                {
                    _tempFloodList.Add(_currentTile);
                }

                else if (GridData[_currentTile].surroundingMines > 0 && !_tempIngoreList.Contains(_currentTile))
                {
                    _tempIngoreList.Add(_currentTile);
                }

                RevealTile(_currentTile);
            }

            _index++;
        }
    }

    private void RevealTile(Vector3Int tile)
    {
        GridData[tile].isRevealed = true;

        GameManager.RemainingTiles -= 1;

        GridData[tile].isFlagged = false;

        MainTilemap.SetTile(tile, null);

        FlagTilemap.SetTile(tile, null);
    }

    public IEnumerator ShowMinePosition()
    {
        Vector3Int _currentMine = GameManager.MinesToReveal[0];

        FlagTilemap.SetTile(_currentMine, MineTile);

        GameManager.MinesToReveal.Remove(_currentMine);

        yield return new WaitForSeconds(0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 _center = LevelBounds.center;

        Vector3 _size = (Vector3)LevelBounds.size;
        Gizmos.DrawWireCube(_center, _size);
    }
}
