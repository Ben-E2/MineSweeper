using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Playing,
    GameLost,
    GameWon
}

public class GameManager : MonoBehaviour
{
    public GameState CurrentGameState;

    [SerializeField] private LevelManager LevelManager;

    public int RemainingFlags;
    public int AmountOfMines;
    public int RemainingTiles;

    void Update()
    {
        if (CurrentGameState == GameState.GameLost) 
        {
            // Display a game over screen first. Not implemented yet

            LevelManager.CreateNewLevel();

            CurrentGameState = GameState.Playing;
        }
        else if (CurrentGameState == GameState.GameWon)
        {
            // Display a game won screen first. Not implemented yet

            LevelManager.CreateNewLevel();

            CurrentGameState = GameState.Playing;
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
}
