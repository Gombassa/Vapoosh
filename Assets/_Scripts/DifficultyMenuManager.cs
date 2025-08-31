// In DifficultyMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyMenuManager : MonoBehaviour
{
    public void SelectDifficulty(int difficulty)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        GameSettings.Difficulty = (GameSettings.AIDifficulty)difficulty;
        GameSettings.NumberOfPlayers = 2;
        GameSettings.IsVsComputer = true;
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToRules()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        GameSettings.PreviousScene = "DifficultyMenu";
        SceneManager.LoadScene("RulesScene");
    }
}