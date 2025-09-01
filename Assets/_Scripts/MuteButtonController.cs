// In MuteButtonController.cs
using UnityEngine;
using UnityEngine.UI;

public class MuteButtonController : MonoBehaviour
{
    [Header("Mute Button Sprites")]
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateMuteButtonIcon();
    }

    public void OnMuteButtonPressed()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.buttonClickSound);
            AudioManager.Instance.ToggleMusic();
            UpdateMuteButtonIcon();
        }
    }
    
    private void UpdateMuteButtonIcon()
    {
        if (buttonImage != null && AudioManager.Instance != null)
        {
            if (AudioManager.Instance.IsMusicMuted())
            {
                buttonImage.sprite = musicOffSprite;
            }
            else
            {
                buttonImage.sprite = musicOnSprite;
            }
        }
    }
}