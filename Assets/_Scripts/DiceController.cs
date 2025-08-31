// In DiceController.cs
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public int PinkDie1 { get; private set; }
    public int PinkDie2 { get; private set; }
    public int GreenDie1 { get; private set; }
    public int GreenDie2 { get; private set; }
    
    private int pinkSum;
    private int greenSum;

    public bool RollDice()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.diceRollSound);
        PinkDie1 = Random.Range(1, 7);
        PinkDie2 = Random.Range(1, 7);
        GreenDie1 = Random.Range(1, 7);
        GreenDie2 = Random.Range(1, 7);
        pinkSum = PinkDie1 + PinkDie2;
        greenSum = GreenDie1 + GreenDie2;
        return (PinkDie1 == 6 && PinkDie2 == 6 && GreenDie1 == 6 && GreenDie2 == 6);
    }

    public Vector2Int GetCoordinateOption1() { return new Vector2Int(pinkSum, greenSum); }
    public Vector2Int GetCoordinateOption2() { return new Vector2Int(greenSum, pinkSum); }
}