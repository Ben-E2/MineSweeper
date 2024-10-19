using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private GameManager GameManager;

    #region Game Summary Window
    [SerializeField] private GameObject GameSummaryWindow;

    [SerializeField] private TextMeshProUGUI OutcomeText;
    [SerializeField] private TextMeshProUGUI TimeTakenText;
    [SerializeField] private TextMeshProUGUI LevelAreaText;

    [SerializeField] private Button ExitButton;
    [SerializeField] private Button NewGameButton;
    #endregion

    #endregion

    public void UpdateGameSummaryWindow(bool won, float timeTaken, int amountOfMines, bool active)
    {
        if (won)
        {
            OutcomeText.text = "You won!";

            TimeTakenText.enabled = true;

            TimeTakenText.text = $"... and it took you {timeTaken.ToString("F3")} seconds.";

            LevelAreaText.text = $"There were a total of {LevelManager.LevelArea} tiles, and {amountOfMines} mines.";
        }

        else 
        {
            OutcomeText.text = "You lost :(";

            TimeTakenText.enabled = false;

            LevelAreaText.text = $"There were a total of {LevelManager.LevelArea} tiles, and {amountOfMines} mines.";
        }

        EnableGameSummaryWindow(active);
    }

    public void EnableGameSummaryWindow(bool active)
    {
        GameSummaryWindow.SetActive(active);
    }

    public void NewGameButtonClicked()
    {
        EnableGameSummaryWindow(false);

        LevelManager.CreateNewLevel();

        GameManager.CurrentGameState = GameState.Playing;
    }

    public void ExitGame()
    {
        EnableGameSummaryWindow(false);

        GameManager.CurrentGameState = GameState.NotPlaying;

        Debug.LogWarning("Game exited.");
    }
}
