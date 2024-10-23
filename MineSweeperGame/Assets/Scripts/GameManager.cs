using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Playing,
    NotPlaying,
    GameLost,
    GameWon
}

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private TimerScript TimerScript;

    #region Game State Variables
    public GameState CurrentGameState;
    public bool OnGoingGame = false;

    [Space]
    public int RemainingFlags;

    [Space]
    public int AmountOfMines;
    public List<Vector3Int> MinesToReveal;

    [Space]
    public int RemainingTiles;
    #endregion

    #endregion

    void Update()
    {
        if (CurrentGameState == GameState.GameLost) 
        {
            CurrentGameState = GameState.NotPlaying;

            StartCoroutine(OnGameLost());
        }
        else if (CurrentGameState == GameState.GameWon)
        {
            CurrentGameState = GameState.NotPlaying;

            StartCoroutine(OnGameWon());
        }
    }

    public void ChangeFlagAmount(int change)
    {
        RemainingFlags += change;
    }

    public void CheckIfGameWon()
    {
        if (RemainingTiles <= AmountOfMines)
        {
            Debug.LogWarning($"You Win. RT: {RemainingTiles} M#: {AmountOfMines}");

            CurrentGameState = GameState.GameWon;
        }
    }

    private IEnumerator OnGameLost()
    {
        OnGoingGame = false;

        TimerScript.ToggleTimer(false);

        MinesToReveal = new List<Vector3Int>();

        foreach (KeyValuePair<Vector3Int, TileData> tile in LevelManager.GridData)
        {
            Vector3Int _key = tile.Key;
            TileData _value = tile.Value;

            if (_value.isMine && !_value.isRevealed)
            {
                MinesToReveal.Add(_key);
            }
        }

        while (MinesToReveal.Count > 0)
        {
            yield return StartCoroutine(LevelManager.ShowMinePosition());
        }

        yield return StartCoroutine(WaitSeconds(1.5f));

        UIManager.UpdateGameSummaryWindow(false, AmountOfMines, true);

        yield return null;
    }

    private IEnumerator OnGameWon()
    {
        OnGoingGame = false;

        TimerScript.ToggleTimer(false);

        yield return StartCoroutine(WaitSeconds(1.5f));

        UIManager.UpdateGameSummaryWindow(true, AmountOfMines, true);

        yield return null;
    }

    public IEnumerator WaitSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
