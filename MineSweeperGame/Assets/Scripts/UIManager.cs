using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private LevelManager LevelManager;
    [SerializeField] private GameManager GameManager;

    #region Game Summary Window
    [SerializeField] private GameObject GameSummaryWindow;

    [SerializeField] private TextMeshProUGUI GSW_OutcomeText;
    [SerializeField] private TextMeshProUGUI GSW_TimeTakenText;
    [SerializeField] private TextMeshProUGUI GSW_LevelAreaText;

    [SerializeField] private Button GSW_ExitButton;
    [SerializeField] private Button GSW_NewGameButton;
    #endregion

    #region Menu Window
    public GameObject MenuWindow;

    [SerializeField] private Button MW_ExitButton;
    [SerializeField] private Button MW_NewGameButton;
    [SerializeField] private Button MW_ReturnToGameButton;
    [SerializeField] private Button MW_SettingsButton;
    #endregion

    #endregion

    public void UpdateGameSummaryWindow(bool won, float timeTaken, int amountOfMines, bool active)
    {
        if (won)
        {
            GSW_OutcomeText.text = "You won!";

            GSW_TimeTakenText.enabled = true;

            GSW_TimeTakenText.text = $"... and it took you {timeTaken.ToString("F3")} seconds.";

            GSW_LevelAreaText.text = $"There were a total of {LevelManager.LevelArea} tiles, and {amountOfMines} mines.";
        }

        else
        {
            GSW_OutcomeText.text = "You lost :(";

            GSW_TimeTakenText.enabled = false;

            GSW_LevelAreaText.text = $"There were a total of {LevelManager.LevelArea} tiles, and {amountOfMines} mines.";
        }

        EnableGameSummaryWindow(active);
    }

    public void EnableGameSummaryWindow(bool active)
    {
        GameSummaryWindow.SetActive(active);
    }

    public void EnableMenuWindow(bool active)
    {
        if (GameManager.OnGoingGame) 
        {
            MW_ReturnToGameButton.gameObject.SetActive(true);
        }

        else
        {
            MW_ReturnToGameButton.gameObject.SetActive(false);
        }

        MenuWindow.SetActive(active);
    }

    public void NewGameButtonClicked()
    {
        EnableGameSummaryWindow(false);

        EnableMenuWindow(false);

        LevelManager.CreateNewLevel();

        GameManager.CurrentGameState = GameState.Playing;
    }

    public void ReturnToGame()
    {
        if (GameManager.OnGoingGame)
        {
            MenuWindow.SetActive(false);
        }

        else
        {
            LevelManager.CreateNewLevel();
        }
    }

    public void SettingsButtonClicked()
    {
        Debug.LogWarning("Not implemented");
    }

    public void ExitGame()
    {
        EnableGameSummaryWindow(false);

        GameManager.CurrentGameState = GameState.NotPlaying;

        Debug.LogWarning("Game exited.");
    }
}
