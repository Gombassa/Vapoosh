// In BoardManager.cs
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] playerCounterPrefabs;

    private GameObject[,] boardTiles;
    private int[,] boardState;
    private GameObject[,] counterObjects;

    public void CreateBoard()
    {
        foreach (Transform child in transform) { Destroy(child.gameObject); }
        boardTiles = new GameObject[12, 12];
        boardState = new int[12, 12];
        counterObjects = new GameObject[12, 12];
        for (int x = 0; x < 12; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, 11 - y, 0), Quaternion.identity);
                tile.name = $"Tile_{x}_{y}";
                tile.transform.parent = this.transform;
                Tile tileScript = tile.GetComponentInChildren<Tile>();
                tileScript.x = x;
                tileScript.y = y;
                int tileNumber = (x + 1) * (y + 1);
                tileScript.numberText.text = tileNumber.ToString();
                boardTiles[x, y] = tile;
                boardState[x, y] = 0;
            }
        }
    }

    public void PlaceCounter(int x, int y, int playerID)
    {
        bool isCapture = false;
        if (counterObjects[x, y] != null)
        {
            if (boardState[x, y] != playerID) { isCapture = true; }
            Destroy(counterObjects[x, y]);
        }

        if (isCapture) { AudioManager.Instance.PlaySound(AudioManager.Instance.captureSound); }
        else { AudioManager.Instance.PlaySound(AudioManager.Instance.placeCounterSound); }

        Tile tileScript = boardTiles[x, y].GetComponentInChildren<Tile>();
        if (tileScript != null)
        {
            tileScript.numberText.color = new Color(0.2f, 0.2f, 0.2f);
            tileScript.numberText.fontStyle = FontStyles.Bold;
        }
        GameObject counterToPlace = playerCounterPrefabs[playerID - 1];
        Vector3 counterPosition = new Vector3(boardTiles[x, y].transform.position.x, boardTiles[x, y].transform.position.y, -0.2f);
        GameObject newCounter = Instantiate(counterToPlace, counterPosition, Quaternion.identity);
        newCounter.transform.rotation = Quaternion.Euler(90, 0, 0);
        boardState[x, y] = playerID;
        counterObjects[x, y] = newCounter;
    }

    public int GetTileState(int x, int y)
    {
        if (x >= 0 && x < 12 && y >= 0 && y < 12) { return boardState[x, y]; }
        return -1;
    }
    
    public void Test_SetState(int x, int y, int state)
    {
        if (x >= 0 && x < 12 && y >= 0 && y < 12) { boardState[x,y] = state; }
    }
}