// In VapooshEffectController.cs
using UnityEngine;
using TMPro;

public class VapooshEffectController : MonoBehaviour
{
    public TextMeshProUGUI vapooshText;
    public float scaleSpeed = 2f;
    public float fadeSpeed = 0.5f;

    void Update()
    {
        // Scale the text up over time
        vapooshText.rectTransform.localScale += Vector3.one * scaleSpeed * Time.deltaTime;

        // Fade the text out over time
        Color textColor = vapooshText.color;
        textColor.a -= fadeSpeed * Time.deltaTime;
        vapooshText.color = textColor;
    }
}