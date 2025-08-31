// In MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame(int players)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        GameSettings.NumberOfPlayers = players;
        GameSettings.IsVsComputer = false;
        SceneManager.LoadScene("GameScene");
    }

    public void StartSinglePlayer()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        SceneManager.LoadScene("DifficultyMenu");
    }
    
    public void GoToRules()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        GameSettings.PreviousScene = "MainMenu";
        SceneManager.LoadScene("RulesScene");
    }
}