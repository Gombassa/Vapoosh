// In RulesManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class RulesManager : MonoBehaviour
{
    public void BackButtonPressed()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
        
        // --- UPDATED: Resume time BEFORE changing scenes ---
        Time.timeScale = 1f;

        if (SceneManager.GetSceneByName("GameScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("RulesScene");
        }
        else
        {
            string sceneToLoad = string.IsNullOrEmpty(GameSettings.PreviousScene) ? "MainMenu" : GameSettings.PreviousScene;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
