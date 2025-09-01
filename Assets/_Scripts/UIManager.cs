// In UIManager.cs
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject vapooshEffectPrefab;
    [SerializeField] private DiceDisplayController diceDisplay;
    public GameObject gameUIContainer;

    [Header("Player Colors")]
    [SerializeField] private Color[] playerColors;

    public void UpdateStatusText(string message, int playerID)
    {
        statusText.text = message;
        if (playerID > 0 && playerID <= playerColors.Length)
        {
            statusText.color = playerColors[playerID - 1];
        }
        else
        {
            statusText.color = Color.white;
        }
    }

    public void UpdateDiceDisplay(int p1, int p2, int g1, int g2)
    {
        if(diceDisplay != null) diceDisplay.UpdateDiceDisplay(p1, p2, g1, g2);
    }
    
    public void HideDiceDisplay()
    {
        if(diceDisplay != null) diceDisplay.HideDiceDisplay();
    }

    public void ShowVapooshEffect()
    {
        if (vapooshEffectPrefab != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.vapooshSound);
            GameObject effect = Instantiate(vapooshEffectPrefab, transform);
            Destroy(effect, 3f);
        }
    }
    
    public void ToggleGameUI(bool show)
    {
        if (gameUIContainer != null)
        {
            gameUIContainer.SetActive(show);
        }
    }

    public void OnRestartButtonPressed()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnBackToMenuButtonPressed()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRulesButtonPressed()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        GameSettings.PreviousScene = "GameScene";
        
        Time.timeScale = 0f;
        ToggleGameUI(false);
        SceneManager.LoadScene("RulesScene", LoadSceneMode.Additive);
    }
}