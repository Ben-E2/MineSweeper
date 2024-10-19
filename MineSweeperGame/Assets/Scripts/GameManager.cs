using UnityEngine;

public enum GameState {
    Playing,
    GameLost,
    GameWon
}

public class GameManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private LevelManager LevelManager;

    #region Game State Variables
    public GameState CurrentGameState;

    [Space]
    public int RemainingFlags;

    [Space]
    public int AmountOfMines;

    [Space]
    public int RemainingTiles;
    #endregion

    #endregion

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
