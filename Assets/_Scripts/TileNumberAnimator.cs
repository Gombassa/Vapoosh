// In TileNumberAnimator.cs
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TileNumberAnimator : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    private TMP_TextInfo textInfo;

    public Color[] gradientColors = new Color[]
    {
        new Color(1f, 0.41f, 0.7f), // UV Pink
        Color.yellow,
        Color.red,
        Color.green,
        new Color(0.25f, 0.88f, 0.82f), // Turquoise
        new Color(0.8f, 0.6f, 0.8f), // Lilac
        Color.blue
    };

    public float animationSpeed = 1.0f;
    public float waveFrequency = 0.5f;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        AnimateVertexColors();
    }

    void AnimateVertexColors()
    {
        textComponent.ForceMeshUpdate();
        textInfo = textComponent.textInfo;

        int characterCount = textInfo.characterCount;
        if (characterCount == 0) return;

        for (int i = 0; i < characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

            for (int j = 0; j < 4; j++)
            {
                Vector3 vertexPosition = textInfo.meshInfo[materialIndex].vertices[vertexIndex + j];
                float waveOffset = vertexPosition.x * waveFrequency;
                float colorPhase = (Time.time * animationSpeed + waveOffset) % 1;
                vertexColors[vertexIndex + j] = GetGradientColor(colorPhase);
            }
        }
        
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    private Color32 GetGradientColor(float t)
    {
        t = Mathf.Clamp01(t);
        float scaledT = t * (gradientColors.Length - 1);
        int colorIndex = Mathf.FloorToInt(scaledT);
        float lerpFactor = scaledT - colorIndex;

        if (colorIndex >= gradientColors.Length - 1)
        {
            return gradientColors[gradientColors.Length - 1];
        }

        return Color.Lerp(gradientColors[colorIndex], gradientColors[colorIndex + 1], lerpFactor);
    }
}