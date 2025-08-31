// In AIController.cs
using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{
    public void TakeTurn(Vector2Int option1, Vector2Int option2, int state1, int state2, int aiPlayerID)
    {
        StartCoroutine(TakeTurnRoutine(option1, option2, state1, state2, aiPlayerID));
    }

    private IEnumerator TakeTurnRoutine(Vector2Int option1, Vector2Int option2, int state1, int state2, int aiPlayerID)
    {
        yield return new WaitForSeconds(1.5f);
        Vector2Int chosenMove;
        switch (GameSettings.Difficulty)
        {
            case GameSettings.AIDifficulty.Easy:
                chosenMove = GetEasyMove(option1, option2);
                break;
            case GameSettings.AIDifficulty.Medium:
                chosenMove = GetMediumMove(option1, option2, state1, state2, aiPlayerID);
                break;
            case GameSettings.AIDifficulty.Hard:
                chosenMove = GetHardMove(option1, option2, state1, state2, aiPlayerID);
                break;
            default:
                chosenMove = GetEasyMove(option1, option2);
                break;
        }
        GameManager.Instance.ProcessPlayerMove(chosenMove.x - 1, chosenMove.y - 1);
    }

    private Vector2Int GetEasyMove(Vector2Int option1, Vector2Int option2)
    {
        return (Random.Range(0, 2) == 0) ? option1 : option2;
    }

    private Vector2Int GetMediumMove(Vector2Int option1, Vector2Int option2, int state1, int state2, int aiPlayerID)
    {
        int humanPlayerID = 1;
        Vector2Int idx1 = new Vector2Int(option1.x - 1, option1.y - 1);
        Vector2Int idx2 = new Vector2Int(option2.x - 1, option2.y - 1);
        if (GameManager.Instance.TestMoveForWin(idx1.x, idx1.y, aiPlayerID)) return option1;
        if (GameManager.Instance.TestMoveForWin(idx2.x, idx2.y, aiPlayerID)) return option2;
        if (GameManager.Instance.TestMoveForWin(idx1.x, idx1.y, humanPlayerID)) return option1;
        if (GameManager.Instance.TestMoveForWin(idx2.x, idx2.y, humanPlayerID)) return option2;
        bool isCapture1 = (state1 != 0 && state1 != aiPlayerID);
        bool isCapture2 = (state2 != 0 && state2 != aiPlayerID);
        if (isCapture1 && !isCapture2) return option1;
        if (!isCapture1 && isCapture2) return option2;
        return GetEasyMove(option1, option2);
    }

    private Vector2Int GetHardMove(Vector2Int option1, Vector2Int option2, int state1, int state2, int aiPlayerID)
    {
        int humanPlayerID = 1;
        int score1 = CalculateMoveScore(option1, state1, aiPlayerID, humanPlayerID);
        int score2 = CalculateMoveScore(option2, state2, aiPlayerID, humanPlayerID);
        if (score1 > score2) return option1;
        if (score2 > score1) return option2;
        return GetEasyMove(option1, option2);
    }

    private int CalculateMoveScore(Vector2Int move, int moveState, int aiPlayerID, int humanPlayerID)
    {
        int score = 0;
        Vector2Int idx = new Vector2Int(move.x - 1, move.y - 1);
        if (GameManager.Instance.TestMoveForWin(idx.x, idx.y, aiPlayerID)) score += 1000;
        if (GameManager.Instance.TestMoveForWin(idx.x, idx.y, humanPlayerID)) score += 500;
        if (GameManager.Instance.TestMoveForThreat(idx.x, idx.y, aiPlayerID)) score += 50;
        if (GameManager.Instance.TestMoveForThreat(idx.x, idx.y, humanPlayerID)) score += 40;
        bool isCapture = (moveState != 0 && moveState != aiPlayerID);
        if (isCapture) score += 10;
        if(moveState == 0) score += 1;
        return score;
    }
}