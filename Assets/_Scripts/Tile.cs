// In Tile.cs
using UnityEngine;
using TMPro; // Add this namespace for TextMeshPro

public class Tile : MonoBehaviour
{
    // Public variables to hold the tile's position in our grid system.
    public int x;
    public int y;

    // A reference to the text component that will display the number.
    public TextMeshProUGUI numberText;
}
