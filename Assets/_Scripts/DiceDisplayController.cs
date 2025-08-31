// In DiceDisplayController.cs
using UnityEngine;
using UnityEngine.UI;

public class DiceDisplayController : MonoBehaviour
{
    [SerializeField] private Image pinkDie1Image, pinkDie2Image, greenDie1Image, greenDie2Image;
    [SerializeField] private Sprite[] pinkDiceSprites; // Assign size 6 in inspector
    [SerializeField] private Sprite[] greenDiceSprites; // Assign size 6 in inspector

    public void UpdateDiceDisplay(int p1, int p2, int g1, int g2)
    {
        gameObject.SetActive(true);
        pinkDie1Image.sprite = pinkDiceSprites[p1 - 1];
        pinkDie2Image.sprite = pinkDiceSprites[p2 - 1];
        greenDie1Image.sprite = greenDiceSprites[g1 - 1];
        greenDie2Image.sprite = greenDiceSprites[g2 - 1];
    }

    public void HideDiceDisplay()
    {
        gameObject.SetActive(false);
    }
}